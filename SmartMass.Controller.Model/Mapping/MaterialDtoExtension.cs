using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Model.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.Mapping
{
    public static class MaterialDtoExtension
    {
        public static void CreateFrom(this MaterialDTO dto, Material pageModel)
        {
            dto.Map(pageModel);
            dto.Created = DateTime.UtcNow;
            dto.Updated = DateTime.UtcNow;
        }

        public static void MapFrom(this MaterialDTO dto, Material pageModel)
        {
            dto.Map(pageModel);
            dto.Id = pageModel.Id;
            dto.Updated = DateTime.UtcNow;
        }

        public static Material MapTo(this MaterialDTO dto)
        {
            return new Material()
            {
                Id = dto.Id,
                Name = dto.Type,
                DefaultBedTemp = dto.DefaultBedTemp,
                DefaultNozzleTemp = dto.DefaultNozzleTemp
            };
        }

        private static void Map(this MaterialDTO dto, Material pageModel)
        {
            dto.Type = pageModel.Name;
            dto.DefaultBedTemp = pageModel.DefaultBedTemp;
            dto.DefaultNozzleTemp = pageModel.DefaultNozzleTemp;
        }
    }
}
