namespace CleanArchTemplate.Application.Common.Models.Chat
{
    public class Message
    {
        public required string ToUserId { get; set; }
        public required string FromUserId { get; set; }
        public required string MessageText { get; set; }
    }
}