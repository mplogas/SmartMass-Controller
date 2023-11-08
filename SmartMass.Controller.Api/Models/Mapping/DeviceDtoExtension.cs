using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Models.Mapping
{
    public static class DeviceDtoExtension
    {
        public static void MapFrom(this DeviceDto dto, Device pageModel)
        {
            dto.Map(pageModel);
            dto.Id = pageModel.Id;
            dto.Updated = DateTime.UtcNow;
        }

        public static void CreateFrom(this DeviceDto dto, Device pageModel)
        {
            dto.Map(pageModel);
            dto.Created = DateTime.UtcNow;
            dto.Updated = DateTime.UtcNow;
        }

        public static Device MapTo(this DeviceDto dto)
        {
            return new Device()
            {
                Id = dto.Id,
                Name = dto.Name,
                ClientId = dto.ClientId,
                CalibrationFactor = dto.CalibrationFactor,
                ScaleCalibrationWeight = dto.ScaleCalibrationWeight,
                ScaleSamplingSize = dto.ScaleSamplingSize,
                ScaleUpdateInterval = dto.ScaleUpdateInterval,
                ScaleDisplayTimeout = dto.ScaleDisplayTimeout / 1000,
                RfidTagDecay = dto.RfidDecay
            };
        }

        private static void Map(this DeviceDto dto, Device pageModel)
        {
            dto.Name = pageModel.Name;
            dto.ClientId = pageModel.ClientId;
            dto.CalibrationFactor = pageModel.CalibrationFactor;
            dto.ScaleCalibrationWeight = pageModel.ScaleCalibrationWeight;
            dto.ScaleSamplingSize = pageModel.ScaleSamplingSize;
            dto.ScaleUpdateInterval = pageModel.ScaleUpdateInterval;
            dto.ScaleDisplayTimeout = pageModel.ScaleDisplayTimeout * 1000;
            dto.RfidDecay = pageModel.RfidTagDecay;
        }
    }
}
