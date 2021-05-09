namespace Tragate.Common.Library
{
    public class ConfigSettings
    {
        public string RedisUrl { get; set; }
        public string ApiUrl { get; set; }
        public string ElasticsearchUrl { get; set; }
        public string SmtpHostName { get; set; }
        public string SmtpDomainName { get; set; }
        public string SmtpQuoteDomainName { get; set; }
        public string SmtpDisplayName { get; set; }
        public string SmtpQuoteDisplayName { get; set; }
        public string SmtpPassword { get; set; }
        public string WebSite { get; set; }
        public S3 S3 { get; set; }
        public string ImageResizerApiUrl { get; set; }
    }

    public class S3
    {
        public string EmailPath { get; set; }
        public string UploadPath { get; set; }
        public string FullImagePath { get; set; }
        public string ImagePath { get; set; }
        public string CDN { get; set; }
    }

    public class Aws
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}