using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Api.Models.Mapping;
using SmartMass.Controller.Api.Services;
using SmartMass.Controller.Mqtt;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly SmartMassDbContext dbContext;
    private readonly ILogger<DevicesController> logger;
    private readonly IMqttClient mqttClient;
    private readonly string mqttTopicBase = string.Empty;
    private readonly IDiscoveredDevices discoveredDevices;

    public DevicesController(ILogger<DevicesController> logger, IConfiguration config, SmartMassDbContext dbContext,
        IMqttClient mqttClient, IDiscoveredDevices discoveredDevices)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.mqttClient = mqttClient;
        mqttTopicBase = config.GetValue<string>("mqtt:topic");
        this.discoveredDevices = discoveredDevices;
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


    // GET: api/<DevicesController>
    [HttpGet("discovered")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<string>> GetDiscovered()
    {
        if (this.discoveredDevices != null)
        {
            return new JsonResult(this.discoveredDevices.GetAll());
        }

        return Problem("no handle");
    }

    // GET: api/<DevicesController>/device_id
    [HttpGet("{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Device>> GetByDeviceId(string deviceId)
    {
        if (!string.IsNullOrWhiteSpace(deviceId))
        {
            if (dbContext.Devices != null)
            {
                var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.ClientId == deviceId);
                if (device != null)
                    return device.MapTo();
                return NotFound();
            }

            return Problem("no context");
        }
        else return BadRequest();
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
            var dto = new DeviceDto();
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

            return CreatedAtAction(nameof(Get), new { device_id = dto.ClientId }, device);
        }

        return BadRequest(ModelState);
    }

    // PUT api/<DevicesController>/5
    [HttpPut("{deviceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(string deviceId, [FromBody] Device device)
    {
        if (deviceId != device.ClientId) return BadRequest();

        if (ModelState.IsValid)
        {
            var dto = await GetDevice(deviceId);
            if(dto == null) return NotFound();

            dto.MapFrom(device);
            dbContext.Entry(dto).State = EntityState.Modified;
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Problem(e.Message);
            }

            return NoContent();
        }

        return BadRequest(ModelState);
    }

    // DELETE api/<DevicesController>/5
    [HttpDelete("{deviceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string deviceId)
    {
        var dto = await GetDevice(deviceId);
        if (dto == null) return NotFound();

        dbContext.Devices.Remove(dto);
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

    [HttpPost("configure/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Configure(string deviceId)
    {
        try
        {
            var dto = await GetDevice(deviceId);
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
                },
                rfid = new
                {
                    decay = dto.RfidDecay
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

    [HttpPost("tare/{deviceID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Tare(string deviceId)
    {
        try
        {
            var dto = await GetDevice(deviceId);
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

    [HttpPost("calibrate/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Calibrate(string deviceId)
    {
        try
        {
            var dto = await GetDevice(deviceId);
            if (dto == null) return NotFound();

            dynamic dynObj = new
            {
                action = "calibrate"
            };
            
            mqttClient.Publish(Helper.BuildMqttTopic(mqttTopicBase, "command", dto.ClientId), JsonConvert.SerializeObject(dynObj));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    private bool DeviceExists(string deviceId)
    {
        return dbContext.Devices.Any(e => e.ClientId == deviceId);
    }

    private async Task<DeviceDto> GetDevice(string deviceId)
    {
        if (DeviceExists(deviceId))
        {
            return await dbContext.Devices.FirstOrDefaultAsync(d => d.ClientId == deviceId);
        }

        return null;
    }
}