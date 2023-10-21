using Microsoft.AspNetCore.Mvc;

namespace SmartMass.Controller.Web.Controllers
{
    public class DevicesController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
