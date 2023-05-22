namespace CleanArchTemplate.Application.Common.Configurations
{
    public class AppConfiguration
    {
        public required string Secret { get; set; }

        public bool BehindSSLProxy { get; set; }

        public  string? ProxyIP { get; set; }

        public required string ApplicationUrl { get; set; }
    }
}