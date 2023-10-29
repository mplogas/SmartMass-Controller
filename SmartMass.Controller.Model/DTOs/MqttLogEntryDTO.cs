using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.DTOs
{
    public class MqttLogEntryDTO
    {
        public MqttLogEntryDTO(long id, Guid spoolId, long value, DateTime received)
        {
            Id = id;
            SpoolId = spoolId;
            Value = value;
            Received = received;
        }
        public long Id { get; private set; }
        public Guid SpoolId { get; private set; }
        public long Value { get; private set; }
        public DateTime Received { get; private set; }
    }
}
