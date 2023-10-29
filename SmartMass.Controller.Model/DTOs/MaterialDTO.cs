using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.DTOs
{
    public class MaterialDTO
    {

        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;

        public int DefaultNozzleTemp { get; set; } = 0;

        public int DefaultBedTemp { get; set; } = 0;

        public List<SpoolDTO> Spools { get; set; } = new();

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
