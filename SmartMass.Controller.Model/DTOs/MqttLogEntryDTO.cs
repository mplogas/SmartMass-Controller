using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.DTOs
{
    public class MqttLogEntryDTO
    {
        public MqttLogEntryDTO(Guid spoolId, long value, DateTime received)
        {
            SpoolId = spoolId;
            Value = value;
            Received = received;
        }
        public long Id { get; set; }
        public Guid SpoolId { get; set; }
        public long Value { get; set; }
        public DateTime Received { get; set; }
    }
}
