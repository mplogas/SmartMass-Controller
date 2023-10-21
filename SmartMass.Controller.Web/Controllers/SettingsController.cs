using Microsoft.AspNetCore.Mvc;

namespace SmartMass.Controller.Web.Controllers
{
    public class SettingsController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
