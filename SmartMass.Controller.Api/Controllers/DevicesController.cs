using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Model.Mapping;
using SmartMass.Controller.Model.PageModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartMass.Controller.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly SmartMassDbContext dbContext;
        private readonly ILogger<DevicesController> logger;
        private readonly Mqtt.IMqttClient mqttClient;

        public DevicesController(ILogger<DevicesController> logger, SmartMassDbContext dbContext, Mqtt.IMqttClient mqttClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mqttClient = mqttClient;
        }

        // GET: api/<DevicesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Device>> Get()
        {
            if (dbContext.Devices != null)
            {
                return dbContext.Devices.Select(device => device.MapTo()).ToList();
            }

            return Problem("no context");
        }

        // GET api/<DevicesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Device>> Get(int id)
        {
            if (dbContext.Devices != null)
            {
                var device = await dbContext.Devices.FindAsync(id);
                if (device != null)
                {
                    return device.MapTo();
                }
                else return NotFound();
            }
            else return Problem("no context");
        }

        // POST api/<DevicesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Device device)
        {
            if (ModelState.IsValid)
            {
                var dto = new DeviceDTO();
                dto.CreateFrom(device);
                dbContext.Add(dto);
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return Problem(e.Message);
                }

                return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
            }
            else return BadRequest(ModelState);
        }

        // PUT api/<DevicesController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Device device)
        {
            if(id != device.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                this.dbContext.Entry(device).State = EntityState.Modified;
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (DeviceExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Problem(e.Message);
                    }
                }

                return NoContent();
            }
            else return BadRequest(ModelState);
        }

        // DELETE api/<DevicesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await this.dbContext.Devices.FindAsync(id);
            if (item != null)
            {
                this.dbContext.Devices.Remove(item);
                try
                {
                    await this.dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return Problem(e.Message);
                }

                return NoContent();
            }
            else return NotFound();
        }

        private bool DeviceExists(long id)
        {
            return this.dbContext.Devices.Any(e => e.Id == id);
        }
    }
}
