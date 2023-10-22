using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model
{
    public class Manufacturer
    {
        public int Id { get; private set; }
        [Required]
        [StringLength(16, ErrorMessage = "The manufacturer name should not exceed 16 characters")]
        [Display(Name = "Manufacturer name")]
        public string Name { get; set; }

        public List<Spool> Spools { get; set; } = new List<Spool>();

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
