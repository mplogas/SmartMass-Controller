using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model
{
    public class Manufacturer
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Manufacturer(string name)
        {
            Name = name;
        }

        public Manufacturer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
