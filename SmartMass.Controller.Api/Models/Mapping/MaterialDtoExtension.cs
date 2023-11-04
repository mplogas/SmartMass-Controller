using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Model.PageModels;

namespace SmartMass.Controller.Api.Models.Mapping
{
    public static class MaterialDtoExtension
    {
        public static void CreateFrom(this MaterialDto dto, Material pageModel)
        {
            dto.Map(pageModel);
            dto.Created = DateTime.UtcNow;
            dto.Updated = DateTime.UtcNow;
        }

        public static void MapFrom(this MaterialDto dto, Material pageModel)
        {
            dto.Map(pageModel);
            dto.Id = pageModel.Id;
            dto.Updated = DateTime.UtcNow;
        }

        public static Material MapTo(this MaterialDto dto)
        {
            return new Material()
            {
                Id = dto.Id,
                Name = dto.Type,
                DefaultBedTemp = dto.DefaultBedTemp,
                DefaultNozzleTemp = dto.DefaultNozzleTemp
            };
        }

        private static void Map(this MaterialDto dto, Material pageModel)
        {
            dto.Type = pageModel.Name;
            dto.DefaultBedTemp = pageModel.DefaultBedTemp;
            dto.DefaultNozzleTemp = pageModel.DefaultNozzleTemp;
        }
    }
}
