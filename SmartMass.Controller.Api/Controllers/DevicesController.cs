using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Model.Mapping;
using SmartMass.Controller.Model.PageModels;
using SmartMass.Controller.Mqtt;

namespace SmartMass.Controller.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly SmartMassDbContext dbContext;
    private readonly ILogger<DevicesController> logger;
    private readonly IMqttClient mqttClient;
    private readonly string mqttTopicBase = string.Empty;

    public DevicesController(ILogger<DevicesController> logger, IConfiguration config, SmartMassDbContext dbContext,
        IMqttClient mqttClient)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.mqttClient = mqttClient;
        mqttTopicBase = config.GetValue<string>("mqtt:topic");
    }

    // GET: api/<DevicesController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Device>>> Get()
    {
        if (dbContext.Devices != null) return await dbContext.Devices.Select(device => device.MapTo()).ToListAsync();

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
                return device.MapTo();
            return NotFound();
        }

        return Problem("no context");
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

            device.Id = dto.Id;
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, device);
        }

        return BadRequest(ModelState);
    }

    // PUT api/<DevicesController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] Device device)
    {
        if (id != device.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            var dto = new DeviceDTO();
            dto.MapFrom(device);

            dbContext.Entry(dto).State = EntityState.Modified;
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (DeviceExists(id))
                    return NotFound();
                return Problem(e.Message);
            }

            return NoContent();
        }

        return BadRequest(ModelState);
    }

    // DELETE api/<DevicesController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await dbContext.Devices.FindAsync(id);
        if (item != null)
        {
            dbContext.Devices.Remove(item);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            return NoContent();
        }

        return NotFound();
    }

    [HttpPost("{id}/configure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Configure(int id)
    {
        try
        {
            var dto = await dbContext.Devices.FindAsync(id);
            if (dto == null) return NotFound();

            dynamic dynObj = new
            {
                action = "configure",
                scale = new
                {
                    update_interval = dto.ScaleUpdateInterval,
                    sampling_size = dto.ScaleSamplingSize,
                    calibration = dto.CalibrationFactor,
                    known_weight = dto.ScaleCalibrationWeight
                },
                display = new
                {
                    display_timeout = dto.ScaleDisplayTimeout
                }
            };

            mqttClient.Publish(Helper.BuildMqttTopic(mqttTopicBase, "command", dto.ClientId), JsonConvert.SerializeObject(dynObj));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("{id}/tare")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Tare(int id)
    {
        try
        {
            var dto = await dbContext.Devices.FindAsync(id);
            if (dto == null) return NotFound();

            dynamic dynObj = new
            {
                action = "tare"
            };

            mqttClient.Publish(Helper.BuildMqttTopic(mqttTopicBase,"command", dto.ClientId), JsonConvert.SerializeObject(dynObj));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("{id}/calibrate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Calibrate(int id)
    {
        try
        {
            var dto = await dbContext.Devices.FindAsync(id);
            if (dto == null) return NotFound();

            dynamic dynObj = new
            {
                action = "calibrate"
            };
            
            mqttClient.Publish(Helper.BuildMqttTopic(mqttTopicBase, "command", dto.ClientId), Convert.ToString(dynObj));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    private bool DeviceExists(long id)
    {
        return dbContext.Devices.Any(e => e.Id == id);
    }


}