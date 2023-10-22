using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model
{
    public  class Device
    {
        public int Id { get; private set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name for the device should not exceed 50 characters.")]
        [Display(Name = "Material name")]
        public string Name { get; private set; } //clientid

        [Display(Name = "Calibration factor for the scale")]
        public int CalibrationFactor { get; set; } = 981;

        // TODO: DataAnnotations for remaining properties
        public int ScaleUpdateIntervall { get; set; } = 1000; //milliseconds
        public int ScaleSamplingSize { get; set; } = 1;
        public int ScaleCalibrationWeight { get; set; } = 100;
        public int ScaleDisplayTimeout { get; set; } = 60000; //milliseconds

        public Device(string name)
        {
            Name = name;
        }

        public Device(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
