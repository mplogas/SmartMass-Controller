using System.ComponentModel.DataAnnotations;

namespace SmartMass.Controller.Model
{
    public class Spool
    {
        public Guid Id { get; private set; }
        [Required]
        [StringLength(48, ErrorMessage = "Can only store 48 characters on a tag.")]
        [Display(Name = "Spool name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Spool creation date")]
        public DateTime Created { get; set; }

        [Range(0,1000, ErrorMessage = "The empty spool weight should be between {1} and {2}")]
        [Display(Name = "Weight of the empty spool")]
        public int EmptySpoolWeight { get; set; } = 0;

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        
        [RegularExpression(@"^#([0-9a-f]{6}|[0-9a-f]{3})$", ErrorMessage = "The color should be provided as web/hex color, e.g. #FF0000")]
        [Display(Name = "Color of the filament")]
        public string Color { get; set; }

        [Range(0, 500, ErrorMessage = "The nozzle temp should only be between {1} and {2} °C")]
        [Display(Name = "Recommended nozzle temperature")]
        public int NozzleTemp { get; set; } = 0;

        [Range(0, 150, ErrorMessage = "The bed temp should only be between {1} and {2} °C")]
        [Display(Name = "Recommended bed temperature")]
        public int BedTemp { get; set; } = 0;

        public Spool(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTime.UtcNow;
        }
    }
}