﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.PageModels
{
    public class Device 
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name for the device should not exceed 50 characters.")]
        [Display(Name = "Device name")]
        public string Name { get; set; } = string.Empty; //clientid

        [Required]
        [Display(Name = "Calibration factor for the scale")]
        public int CalibrationFactor { get; set; } = 981;

        [Range(500, 3600000, ErrorMessage = "The sensor update interval should be between {1} and {2}.")]
        [Display(Name = "sensor update interval (in ms)")]
        public int ScaleUpdateInterval { get; set; } = 1000; //milliseconds

        [Range(1, 5, ErrorMessage = "The sensor sampling size should be between {1} and {2} to avoid HX711 read errors.")]
        [Display(Name = "sensor sampling size (values)")]
        public int ScaleSamplingSize { get; set; } = 1;

        [Range(1, 1500, ErrorMessage = "The weight used for calibration should be between {1} and {2}.")]
        [Display(Name = "calibrated weight")]
        public int ScaleCalibrationWeight { get; set; } = 100;

        [Range(1, 21600, ErrorMessage = "The display timeout should be between {1} and {2} to avoid burn-in.")]
        [Display(Name = "display timeout (in s)")]
        public int ScaleDisplayTimeout { get; set; } = 60; //seconds!
    }
}