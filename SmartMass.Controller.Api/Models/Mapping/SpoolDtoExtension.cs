using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Model.PageModels;

namespace SmartMass.Controller.Api.Models.Mapping
{
    public static class SpoolDtoExtension
    {
        public static void CreateFrom(this SpoolDto dto, Spool pageModel)
        {
            dto.Map(pageModel);
            dto.Id = Guid.NewGuid();
            dto.Created = DateTime.UtcNow;
            dto.Updated = DateTime.UtcNow;
        }

        public static void MapFrom(this SpoolDto dto, Spool pageModel)
        {
            dto.Map(pageModel);
            dto.Id = pageModel.Id;
            dto.Updated = DateTime.UtcNow;
        }

        public static Spool MapTo(this SpoolDto dto)
        {
            return new Spool()
            {
                Id = dto.Id,
                Name = dto.Name,
                Color = dto.Color,
                BedTemp = dto.BedTemp,
                NozzleTemp = dto.NozzleTemp,
                EmptySpoolWeight = dto.EmptySpoolWeight,
                ManufacturerId = dto.ManufacturerId,
                ManufacturerName = dto.ManufacturerDto.Name,
                MaterialId = dto.MaterialId,
                MaterialName = dto.MaterialDto.Type,
                Created = dto.Created
            };
        }

        private static void Map(this SpoolDto dto, Spool pageModel)
        {
            dto.Name = pageModel.Name;
            dto.Color = pageModel.Color;
            dto.BedTemp = pageModel.BedTemp;
            dto.NozzleTemp = pageModel.NozzleTemp;
            dto.EmptySpoolWeight = pageModel.EmptySpoolWeight;
            dto.ManufacturerId = pageModel.ManufacturerId;
            dto.MaterialId = pageModel.MaterialId;
            dto.Created = pageModel.Created;
        }
    }
}
