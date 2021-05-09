using System;
using System.Security.Cryptography;
using System.Text;

namespace Tragate.Common.Library
{
    public class PasswordHashHelper
    {
        public static string HashPassword(string passord, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{passord}{salt}"));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        public static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}