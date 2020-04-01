using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shot.Licensing.Api.Services
{
    public static class Utils
    {
        private static readonly HashAlgorithm SHA256 = new SHA256Managed();
        public const string ChecksumType = "sha256";

        public static string GetSha256HashFromString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            byte[] textData = Encoding.UTF8.GetBytes(text);
            return BitConverter.ToString(SHA256.ComputeHash(textData)).Replace("-", string.Empty);
        }
    }
}
