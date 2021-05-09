using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SlugityLib;
using SlugityLib.Configuration;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using static System.String;

namespace Tragate.Common.Library.Helpers
{
    public static class Helper
    {
        private static IConfiguration _configuration;

        private static IConfiguration Config(){
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true);

            _configuration = builder.Build();
            return _configuration;
        }

        public static string GenerateSlug(this string value){
            var configuration = new SlugityConfig
            {
                TextCase = TextCase.LowerCase,
                StringSeparator = '-'
            };

            configuration.ReplacementCharacters.Add("m³", "-");
            configuration.ReplacementCharacters.Add("m²", "-");
            configuration.ReplacementCharacters.Add("½", "-");

            var slugity = new Slugity(configuration);
            var unaccentedText = Join("", value.Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
            return slugity.GenerateSlug(unaccentedText);
        }

        //<Summary>
        /// http://vunvulearadu.blogspot.com.tr/2013/04/steamcopyto-performance-problems.html
        //</Summary
        public static MemoryStream ConvertToBinary(this Stream value){
            byte[] bytes;
            using (var reader = new BinaryReader(value)){
                bytes = reader.ReadBytes((int) value.Length);
            }

            return new MemoryStream(bytes, 0, bytes.Count());
        }

        public static AWSOptions GetConfig(this AWSOptions value, string accesskey,
            string secretKey){
            value.Credentials = new BasicAWSCredentials(accesskey, secretKey);
            return value;
        }

        public static string CheckUserProfileImage(this string profileImage){
            if (!IsNullOrEmpty(profileImage))
                return Config()["S3:CDN"] + profileImage;
            return $"{Config()["S3:CDN"]}items/person.jpg";
        }

        public static string CheckProductProfileImage(this string profileImage){
            if (!IsNullOrEmpty(profileImage))
                return Config()["S3:CDN"] + profileImage;
            return $"{Config()["S3:CDN"]}items/product.jpg";
        }

        public static string CheckCompanyProfileImage(this string profileImage){
            if (!IsNullOrEmpty(profileImage) && !profileImage.Contains("items")){
                return profileImage;
            }

            if (!IsNullOrEmpty(profileImage)){
                return Config()["S3:CDN"] + profileImage;
            }

            return $"{Config()["S3:CDN"]}items/company.jpg";
        }

        public static string CheckCategoryProfileImage(this string imagePath){
            if (!IsNullOrEmpty(imagePath))
                return Config()["S3:CDN"] + imagePath;
            return $"{Config()["S3:CDN"]}items/none-image.jpg";
        }

        public static List<ImageFileDto> ConvertImageFile(this IFormFileCollection files, string name){
            var r = new Random();
            var Id = r.Next(5, 100000);
            return files.Select(file => new ImageFileDto()
                {
                    Key = $"{Config()["S3:ImagePath"]}{name}-{Id}.jpg",
                    ContentType = file.ContentType,
                    File = file
                })
                .ToList();
        }

        public static bool CheckFileExtensions(this string value){
            string[] extensions = {".jpeg", ".jpg", ".png", ".bmp", ".gif"};
            return extensions.Contains(value.ToLower());
        }

        public static string GetUserImageName(string fullName, UserType userType){
            var r = new Random();
            var Id = r.Next(5, 100000);
            var fileName = userType == UserType.Company
                ? $"{Config()["S3:ImagePath"]}{fullName.GenerateSlug()}-{Id}.jpg"
                : $"{Config()["S3:ImagePath"]}{Guid.NewGuid()}.jpg";
            return fileName;
        }

        public static string GetCategoryImageName(IFormFile file){
            return $"{Config()["S3:ImagePath"]}{Guid.NewGuid()}.jpg";
        }

        public static string GetOldImagePath(this string value){
            return !IsNullOrEmpty(value) ? value.Split('/')[1] : null;
        }
    }
}