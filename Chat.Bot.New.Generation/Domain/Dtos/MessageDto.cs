namespace Domain.Dtos
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public string Text { get; set; }
        public DateTime DtInserted { get; set; }
    }
}
