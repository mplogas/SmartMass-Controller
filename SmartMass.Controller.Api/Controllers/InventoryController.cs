using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Api.Models.Mapping;
using SmartMass.Controller.Mqtt;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> logger;
        private readonly SmartMassDbContext dbContext;
        private readonly IMqttClient mqttClient;
        private readonly string mqttTopicBase = string.Empty;

        public InventoryController(ILogger<InventoryController> logger, IConfiguration config, SmartMassDbContext dbContext, Mqtt.IMqttClient mqttClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mqttClient = mqttClient;
            mqttTopicBase = config.GetValue<string>("mqtt:topic");
        }

        // GET: api/<InventoryController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Spool>>> Get()
        {
            if (dbContext.Spools != null) return await dbContext.Spools.Include(m => m.ManufacturerDto).Include(m => m.MaterialDto).Select(m => m.MapTo()).ToListAsync();

            return Problem("no context");
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Spool>> Get(Guid id)
        {
            if (dbContext.Spools != null)
            {
                var dto = await dbContext.Spools.Include(m => m.ManufacturerDto).Include(m => m.MaterialDto)
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (dto != null)
                    return dto.MapTo();
                return NotFound();
            }

            return Problem("no context");
        }

        // POST api/<InventoryController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Spool spool)
        {
            if (ModelState.IsValid)
            {
                var manufacturerDto = await dbContext.Manufacturers.FindAsync(spool.ManufacturerId);
                var materialDto = await dbContext.Materials.FindAsync(spool.MaterialId);

                if (manufacturerDto != null && materialDto != null)
                {
                    var dto = new SpoolDto();
                    dto.CreateFrom(spool);
                    dto.ManufacturerDto = manufacturerDto;
                    dto.MaterialDto = materialDto;
                    dbContext.Add(dto);

                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        logger.LogError("Failed to create entry", e);
                        return Problem(e.Message);
                    }

                    spool.Id = dto.Id;
                    return CreatedAtAction(nameof(Get), new { id = dto.Id }, spool);

                }
                else return BadRequest();

            }

            return BadRequest(ModelState);
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] Spool spool)
        {
            if (id != spool.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var dto = new SpoolDto();
                dto.MapFrom(spool);

                dbContext.Entry(dto).State = EntityState.Modified;
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (SpoolExists(id))
                        return NotFound();
                    
                    logger.LogError("Failed to update entry", e);
                    return Problem(e.Message);
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await dbContext.Spools.FindAsync(id);
            if (dto != null)
            {
                dbContext.Spools.Remove(dto);
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    logger.LogError("Failed to delete entry", e);
                    return Problem(e.Message);
                }

                return NoContent();
            }

            return NotFound();
        }

        [HttpPost("write-tag/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WriteTag(Guid id, string deviceId)
        {
            try
            {
                var dto = await dbContext.Spools.Include(m => m.ManufacturerDto).Include(m => m.MaterialDto)
                    .FirstOrDefaultAsync(s => s.Id == id);

                var dev = await dbContext.Devices.FirstOrDefaultAsync(d => d.ClientId == deviceId);
                if (dto == null || dev == null) return NotFound();

                dynamic dynObj = new
                {
                    action = "write-tag",
                    tag = new
                    {
                        spool_id = dto.Id,
                        spool_weight = dto.EmptySpoolWeight,
                        material = dto.MaterialDto.Type,
                        color = dto.Color,
                        manufacturer = dto.ManufacturerDto.Name,
                        spool_name = dto.Name,
                        timestamp = dto.Created.Ticks
                    }
                };

                mqttClient.Publish(Helper.BuildMqttTopic(mqttTopicBase, "command", dev.ClientId), JsonConvert.SerializeObject(dynObj));
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }


        private bool SpoolExists(Guid id)
        {
            return dbContext.Spools.Any(e => e.Id == id);
        }
    }
}
