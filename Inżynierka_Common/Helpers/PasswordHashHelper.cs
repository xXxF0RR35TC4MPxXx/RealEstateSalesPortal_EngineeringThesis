using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Common.Helpers
{
    public static class PasswordHashHelper
    {
        public static string GetHash(string input)
        {
            SHA256 hashAlgorithm = SHA256.Create();
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string EncodePasswordToBase64(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            string? result = Convert.ToBase64String(bytes);

            return result;
        }

        public static string DecodeFrom64(string base64EncodedData)
        {
            byte[]? base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            string? result = Encoding.UTF8.GetString(base64EncodedBytes);

            return result;
        }
    }
}
