using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

    }
}
