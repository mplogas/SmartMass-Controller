using Microsoft.AspNetCore.Mvc;
using SmartMass.Controller.Mqtt;
using SmartMass.Controller.Web.Data;

namespace SmartMass.Controller.Web.Controllers
{
    public class InventoryController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly SmartMassDbContext dbContext;
        private readonly IMqttClient mqttClient;

        public InventoryController(SmartMassDbContext dbContext, Mqtt.IMqttClient mqttclient)
        {
            this.dbContext = dbContext;
            this.mqttClient = mqttclient;
        }
        public IActionResult Index()
        {
            mqttClient.Publish("smartmass/scale-01", "{\"status\": \"ok\"}");

            return View();
        }
    }
}
