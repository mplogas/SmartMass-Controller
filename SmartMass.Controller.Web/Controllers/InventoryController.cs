using Microsoft.AspNetCore.Mvc;
using SmartMass.Controller.Web.Data;

namespace SmartMass.Controller.Web.Controllers
{
    public class InventoryController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly SmartMassDbContext dbContext;

        public InventoryController(SmartMassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
