using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Shared.Models
{
    public class HistoryEvent
    {
        public Guid SpoolId { get; set; }
        public long Value { get; set; }
        public DateTime Received { get; set; }
    }
}
