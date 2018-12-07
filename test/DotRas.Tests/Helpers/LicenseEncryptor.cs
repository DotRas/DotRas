using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotRas.Tests.Helpers
{
    public class LicenseEncryptor
    {
        private const int IterationCount = 100000;

        private const int BlockSize = 128;
        private const int BufferSize = 256;
        private const int SaltSize = 256;
        private const int KeySize = 256;

        public byte[] EncryptBytes(string licenseKey, string xml)
        {
            var bytes = Encoding.UTF8.GetBytes(xml);
            
            var pbkdf2 = new Rfc2898DeriveBytes(licenseKey, SaltSize, IterationCount);
            var salt = pbkdf2.Salt;

            var csp = new AesCryptoServiceProvider
            {
                KeySize = KeySize,
                BlockSize = BlockSize
            };

            csp.Key = pbkdf2.GetBytes(KeySize / 8);
            csp.IV = pbkdf2.GetBytes(BlockSize / 8);

            var ms = new MemoryStream();
            ms.Write(salt, 0, SaltSize);

            var cryptoStream = new CryptoStream(ms, csp.CreateEncryptor(), CryptoStreamMode.Write);

            var reader = new BinaryReader(new MemoryStream(bytes));
            var buffer = new byte[BufferSize];

            int bytesRead;
            while ((bytesRead = reader.Read(buffer, 0, BufferSize)) > 0)
            {
                cryptoStream.Write(buffer, 0, bytesRead);
            }

            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}