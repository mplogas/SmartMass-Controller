using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Mqtt
{
    public static class Helper
    {
        public static string BuildMqttTopic(string baseTopic, string action, string clientId)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(baseTopic))
            {
                sb.Append(baseTopic);
                if (!baseTopic.EndsWith('/')) sb.Append('/');
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                sb.Append(action);
                if (!action.EndsWith('/'))
                {
                    sb.Append('/');
                }

                if (!string.IsNullOrWhiteSpace(clientId))
                {
                    sb.Append(clientId);
                }
                //TODO: remove before prod :D
                else
                    sb.Append("scale-01");
            }

            return sb.ToString();
        }
    }
}
