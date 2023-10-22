namespace SmartMass.Controller.Model
{
    public class Spool
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public DateTime Created { get; private set; }
        public int EmpytSpoolWeigth { get; set; } = 0;
        public Manufacturer Manufacturer { get; set; }
        public Material Material { get; set; }
        public string Color { get; set; }

        public Spool(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTime.UtcNow;
        }
    }
}