namespace SmartMass.Controller.Api.Models.DTOs
{
    public class MqttLogEntryDto
    {
        public MqttLogEntryDto(Guid spoolId, long value, DateTime received)
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
