using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model
{
    public class Material
    {
        public Material(string type)
        {
            Type = type;
        }

        public Material(int id, string type, int defaultNozzleTemp, int defaultBedTemp)
        {
            Id = id;
            Type = type;
            DefaultNozzleTemp = defaultNozzleTemp;
            DefaultBedTemp = defaultBedTemp;
        }

        public int Id { get; private set; }
        public string Type { get; private set; }
        public int DefaultNozzleTemp { get; private set; } = 0;
        public int DefaultBedTemp { get; private set; } = 0;

    }
}
