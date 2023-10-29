using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Model.Mapping;
using SmartMass.Controller.Model.PageModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartMass.Controller.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturersController : ControllerBase
{
    private readonly SmartMassDbContext dbContext;
    private readonly ILogger<DevicesController> logger;

    public ManufacturersController(ILogger<DevicesController> logger, SmartMassDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    // GET: api/<ManufacturersController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Manufacturer>>> Get()
    {
        if (dbContext.Manufacturers != null) return await dbContext.Manufacturers.Select(m => m.MapTo()).ToListAsync();

        return Problem("no context");
    }

    // GET api/<ManufacturersController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Manufacturer>> Get(int id)
    {
        if (dbContext.Manufacturers != null)
        {
            var manufacturerDto = await dbContext.Manufacturers.FindAsync(id);
            if (manufacturerDto != null)
                return manufacturerDto.MapTo();
            return NotFound();
        }

        return Problem("no context");
    }

    // POST api/<ManufacturersController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] Manufacturer manufacturer)
    {
        if (ModelState.IsValid)
        {
            var dto = new ManufacturerDTO();
            dto.CreateFrom(manufacturer);
            dbContext.Add(dto);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            manufacturer.Id = dto.Id;
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, manufacturer);
        }

        return BadRequest(ModelState);
    }

    // PUT api/<ManufacturersController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] Manufacturer manufacturer)
    {
        if (id != manufacturer.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            var dto = new ManufacturerDTO();
            dto.MapFrom(manufacturer);

            dbContext.Entry(dto).State = EntityState.Modified;
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (ManufacturerExists(id))
                    return NotFound();
                return Problem(e.Message);
            }

            return NoContent();
        }

        return BadRequest(ModelState);
    }

    // DELETE api/<ManufacturersController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await dbContext.Manufacturers.FindAsync(id);
        if (item != null)
        {
            dbContext.Manufacturers.Remove(item);
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

    private bool ManufacturerExists(long id)
    {
        return dbContext.Manufacturers.Any(e => e.Id == id);
    }
}