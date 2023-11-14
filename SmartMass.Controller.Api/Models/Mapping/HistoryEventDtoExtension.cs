using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Models.Mapping
{
    public static class HistoryEventDtoExtension
    {
        public static Event MapTo(this HistoryEventDto dto)
        {
            return new Event()
            {
                SpoolId = dto.SpoolId,
                Value = dto.Value,
                Received = dto.Received
            };
        }
    }
}
