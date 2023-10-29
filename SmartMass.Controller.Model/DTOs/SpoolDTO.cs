using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMass.Controller.Model.DTOs
{
    public class SpoolDTO
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int EmptySpoolWeight { get; set; } = 0;

        public int ManufacturerId { get; set; }
        public ManufacturerDTO ManufacturerDto { get; set; } = new();

        public int MaterialId { get; set; }
        public MaterialDTO MaterialDto { get; set; } = new();

        public string Color { get; set; } = "#000000";

        public int NozzleTemp { get; set; } = 0;

        public int BedTemp { get; set; } = 0;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}