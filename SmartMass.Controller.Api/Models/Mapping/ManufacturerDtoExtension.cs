using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Models.Mapping
{
    public static class ManufacturerDtoExtension
    {
        public static void CreateFrom(this ManufacturerDto dto, Manufacturer pageModel)
        {
            dto.Map(pageModel);
            dto.Created = DateTime.UtcNow;
            dto.Updated = DateTime.UtcNow;
        }

        public static void MapFrom(this ManufacturerDto dto, Manufacturer pageModel)
        {
            dto.Map(pageModel);
            dto.Id = pageModel.Id;
            dto.Updated = DateTime.UtcNow;
        }

        public static Manufacturer MapTo(this ManufacturerDto dto)
        {
            return new Manufacturer()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        private static void Map(this ManufacturerDto dto, Manufacturer pageModel)
        {
            dto.Name = pageModel.Name;
        }
    }
}
