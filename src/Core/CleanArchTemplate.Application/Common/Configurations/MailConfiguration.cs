namespace CleanArchTemplate.Application.Common.Configurations
{
    public class MailConfiguration
    {
        public required string From { get; set; }
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? DisplayName { get; set; }
    }
}