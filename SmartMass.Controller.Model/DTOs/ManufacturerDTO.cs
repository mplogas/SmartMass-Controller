using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.DTOs
{
    public class ManufacturerDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "The manufacturer name should not exceed 16 characters")]
        [Display(Name = "Manufacturer name")]
        public string Name { get; set; } = string.Empty;

        public List<SpoolDTO> Spools { get; set; } = new List<SpoolDTO>();
    }
}
