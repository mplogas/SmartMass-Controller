using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Model.PageModels;

namespace SmartMass.Controller.Model.Mapping
{
    public static class DeviceDTOExtension
    {
        public static void MapFrom(this DeviceDTO dto, Device pageModel)
        {
            dto.Id = pageModel.Id;
            dto.Name = pageModel.Name;
            dto.CalibrationFactor = pageModel.CalibrationFactor;
            dto.ScaleCalibrationWeight = pageModel.ScaleCalibrationWeight;
            dto.ScaleSamplingSize = pageModel.ScaleSamplingSize;
            dto.ScaleUpdateInterval = pageModel.ScaleUpdateInterval;
            dto.ScaleDisplayTimeout = pageModel.ScaleDisplayTimeout * 1000;
        }

        public static Device MapTo(this DeviceDTO dto)
        {
            return new Device()
            {
                Id = dto.Id,
                Name = dto.Name,
                CalibrationFactor = dto.CalibrationFactor,
                ScaleCalibrationWeight = dto.ScaleCalibrationWeight,
                ScaleSamplingSize = dto.ScaleSamplingSize,
                ScaleUpdateInterval = dto.ScaleUpdateInterval,
                ScaleDisplayTimeout = dto.ScaleDisplayTimeout / 1000,
            };
        }
    }
}
