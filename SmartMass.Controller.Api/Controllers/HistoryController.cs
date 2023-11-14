using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class HistoryController : ControllerBase
    {
        private readonly SmartMassDbContext dbContext;
        private readonly ILogger<HistoryController> logger;

        public HistoryController(ILogger<HistoryController> logger, SmartMassDbContext context)
        {
            this.logger = logger;
            this.dbContext = context;
        }

        [HttpGet("{spool_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<HistoryEvent>> GetMqttLogEntryDto(Guid spool_id)
        {
            if (spool_id != Guid.Empty)
            {
                if (dbContext.MqttValues != null)
                {
                    var result = new List<HistoryEvent>();
                    var tmp = dbContext.MqttValues.Where(e => e.SpoolId == spool_id);
                    foreach (var e in tmp)
                    {
                        result.Add(e.MapTo());
                    }

                    if (!result.Any()) return NotFound();
                    else return result;
                }

                return Problem("no context");
            } else return BadRequest();
        }
    }
}
