using System;
using System.Security.Cryptography;

namespace DotRas.Tests.Helpers
{
    public class LicenseKeyGenerator
    {
        public static string GenerateLicenseKey()
        {
            var passwordBytes = new byte[20];

            var p = new RNGCryptoServiceProvider();
            p.GetBytes(passwordBytes);

            return Convert.ToBase64String(passwordBytes);
        }
    }
}