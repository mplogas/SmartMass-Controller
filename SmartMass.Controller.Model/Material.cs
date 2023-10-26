using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model
{
    public class Material
    {

        public int Id { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Use a material abbreviation, e.g. PETG, PAHT-CF, PLA")]
        [Display(Name = "Material name")]
        public string Type { get; set; } = string.Empty;

        [Range(0, 500, ErrorMessage = "The nozzle temp should only be between {1} and {2} °C")]
        [Display(Name = "Default nozzle temperature")]
        public int DefaultNozzleTemp { get; set; } = 0;

        [Range(0, 150, ErrorMessage = "The bed temp should only be between {1} and {2} °C")]
        [Display(Name = "Default bed temperature")]
        public int DefaultBedTemp { get; set; } = 0;

        public List<Spool> Spools { get; set; } = new List<Spool>();

    }
}
