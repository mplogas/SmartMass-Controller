using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Model.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMass.Controller.Model.Mapping
{
    public static class ManufacturerDtoExtension
    {
        public static void CreateFrom(this ManufacturerDTO dto, Manufacturer pageModel)
        {
            dto.Name = pageModel.Name;
        }

        public static void MapFrom(this ManufacturerDTO dto, Manufacturer pageModel)
        {
            dto.CreateFrom(pageModel);
            dto.Id = pageModel.Id;
        }

        public static Manufacturer MapTo(this ManufacturerDTO dto)
        {
            return new Manufacturer()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
