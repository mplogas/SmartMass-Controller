using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Api.Models.Mapping;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> logger;
        private readonly SmartMassDbContext dbContext;

        public MaterialsController(ILogger<MaterialsController> logger, SmartMassDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        // GET: api/<MaterialsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Material>>> Get()
        {
            if (dbContext.Materials != null) return await dbContext.Materials.Select(m => m.MapTo()).ToListAsync();

            return Problem("no context");
        }

        // GET api/<MaterialsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Material>> Get(int id)
        {
            if (dbContext.Materials != null)
            {
                var dto = await dbContext.Materials.FindAsync(id);
                if (dto != null)
                    return dto.MapTo();
                return NotFound();
            }

            return Problem("no context");
        }

        // POST api/<MaterialsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Material material)
        {
            if (ModelState.IsValid)
            {
                var dto = new MaterialDto();
                dto.CreateFrom(material);
                dbContext.Add(dto);
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return Problem(e.Message);
                }

                material.Id = dto.Id;
                return CreatedAtAction(nameof(Get), new { id = dto.Id }, material);
            }

            return BadRequest(ModelState);
        }

        // PUT api/<MaterialsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Material material)
        {
            if (id != material.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var dto = new MaterialDto();
                dto.MapFrom(material);

                dbContext.Entry(dto).State = EntityState.Modified;
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (MaterialExists(id))
                        return NotFound();
                    return Problem(e.Message);
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE api/<MaterialsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await dbContext.Materials.FindAsync(id);
            if (item != null)
            {
                dbContext.Materials.Remove(item);
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

        private bool MaterialExists(long id)
        {
            return dbContext.Materials.Any(e => e.Id == id);
        }
    }
}
