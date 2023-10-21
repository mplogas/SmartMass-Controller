using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Mqtt
{
    public interface IMqttClient
    {
        Task Connect(string broker, string clientId, string username, string password);
        Task Disconnect();
        Task Subscribe(string topic);
        Task Publish(string topic, string message);
    }
}
