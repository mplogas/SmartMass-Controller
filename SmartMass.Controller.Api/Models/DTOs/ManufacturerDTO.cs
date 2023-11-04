namespace SmartMass.Controller.Api.Models.DTOs
{
    public class ManufacturerDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<SpoolDto> Spools { get; set; } = new List<SpoolDto>();

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
