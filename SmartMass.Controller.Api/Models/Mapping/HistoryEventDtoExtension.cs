using SmartMass.Controller.Api.Models.DTOs;
using SmartMass.Controller.Shared.Models;

namespace SmartMass.Controller.Api.Models.Mapping
{
    public static class HistoryEventDtoExtension
    {
        public static HistoryEvent MapTo(this HistoryEventDto dto)
        {
            return new HistoryEvent()
            {
                SpoolId = dto.SpoolId,
                Value = dto.Value,
                Received = dto.Received
            };
        }
    }
}
