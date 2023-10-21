using Microsoft.AspNetCore.Mvc;

namespace SmartMass.Controller.Web.Controllers
{
    public class InventoryController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
