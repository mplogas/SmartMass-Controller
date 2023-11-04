namespace SmartMass.Controller.Api.Models.DTOs
{
    public class MaterialDto
    {

        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;

        public int DefaultNozzleTemp { get; set; } = 0;

        public int DefaultBedTemp { get; set; } = 0;

        public List<SpoolDto> Spools { get; set; } = new();

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
