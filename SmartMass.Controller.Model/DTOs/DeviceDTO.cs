namespace SmartMass.Controller.Model.DTOs
{
    public class DeviceDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;

        public int CalibrationFactor { get; set; } = 981;

        public int ScaleUpdateInterval { get; set; } = 1000; //milliseconds

        public int ScaleSamplingSize { get; set; } = 1;

        public int ScaleCalibrationWeight { get; set; } = 100;

        public int ScaleDisplayTimeout { get; set; } = 60000; //milliseconds!

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

    }
}
