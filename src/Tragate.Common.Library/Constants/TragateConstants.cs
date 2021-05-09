using System;
using Nest;

namespace Tragate.Common.Library.Constants
{
    public class TragateConstants
    {
        public const string ROOT_TYPE = "root";
        public const string PARENT_TYPE = "company";


        public static readonly string COMPANY_DATA = GetEnvironment() == "Development" || GetEnvironment() == "Staging"
            ? "test-companydata"
            : "companydata";

        public static readonly string ALIAS = GetEnvironment() == "Development" || GetEnvironment() == "Staging"
            ? "test-alias-web"
            : "alias-web";

        private static string GetEnvironment(){
            return sd.Fonksiyonlar.Ayarlar["EnvironmentName"]; //Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }
}