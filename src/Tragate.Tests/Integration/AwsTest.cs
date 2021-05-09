using System.Linq;
using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tragate.Tests.Integration
{
    public class AwsTest : TestBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonSimpleEmailService _emailService;

        public AwsTest(){
            var provider = BuildServiceProvider();
            _emailService = provider.GetService<IAmazonSimpleEmailService>();
            _s3Client = provider.GetService<IAmazonS3>();
        }


        [Fact]
        public void Should_Be_List_Bucket_Name_From_S3(){
            var buckets = _s3Client.ListBucketsAsync();
            var response = buckets.Result;
            Assert.Equal(response.HttpStatusCode, HttpStatusCode.OK);
            Assert.True(response.Buckets.Any(x => x.BucketName == "tr-logs"));
            Assert.True(response.Buckets.Any(x => x.BucketName == "cdn.tragate.com"));
        }

        [Fact]
        public void Should_Has_Be_Email_Templates_Files_At_S3(){
            var files = _s3Client.ListObjectsAsync(new ListObjectsRequest {BucketName = "cdn.tragate.com"});
            var response = files.Result;
            Assert.Equal(response.HttpStatusCode, HttpStatusCode.OK);
            Assert.True(response.S3Objects.Any(x => x.Key == "assets/"));
            Assert.True(response.S3Objects.Any(x => x.Key == "assets/activation.html"));
            Assert.True(response.S3Objects.Any(x => x.Key == "assets/quotation-message-update.html"));
            Assert.True(response.S3Objects.Any(x => x.Key == "assets/email-header.png"));
            Assert.True(response.S3Objects.Any(x => x.Key == "assets/reset-password.html"));
        }

        [Fact]
        public void Should_Be_Reach_To_Ses_Account(){
            var result = _emailService.GetAccountSendingEnabledAsync(new GetAccountSendingEnabledRequest()).Result
                .Enabled;
            Assert.True(result);
        }
    }
}