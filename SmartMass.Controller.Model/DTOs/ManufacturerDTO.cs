namespace SmartMass.Controller.Model.DTOs
{
    public class ManufacturerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<SpoolDTO> Spools { get; set; } = new List<SpoolDTO>();
    }
}
