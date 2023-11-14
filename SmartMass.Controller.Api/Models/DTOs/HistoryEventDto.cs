namespace SmartMass.Controller.Api.Models.DTOs
{
    public class HistoryEventDto
    {
        public HistoryEventDto(Guid spoolId, long value, DateTime received)
        {
            SpoolId = spoolId;
            Value = value;
            Received = received;
        }
        public long Id { get; set; }
        public Guid SpoolId { get; set; }
        public long Value { get; set; }
        public DateTime Received { get; set; }
    }
}
