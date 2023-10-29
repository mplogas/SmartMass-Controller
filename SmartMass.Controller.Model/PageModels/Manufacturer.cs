using System.ComponentModel.DataAnnotations;

namespace SmartMass.Controller.Model.PageModels
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "The manufacturer name should not exceed 16 characters")]
        [Display(Name = "Manufacturer name")]
        public string Name { get; set; } = string.Empty;
    }
}
