using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Application.Authentication
{
    public class SecretPasswordHasher : ISecretPasswordHasher
    {
        private readonly IConfiguration _configuration;

        public SecretPasswordHasher()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
        }

        public (byte[] encryptedPassword, byte[] iv) EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = Encoding.ASCII.GetBytes(_configuration["AES:Key"]);

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(password);
            }

            var encrypted = msEncrypt.ToArray();

            return (encrypted, aes.IV);
        }

        public string DecryptPassword(byte[] encryptedPassword, byte[] iv)
        {
            var aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = Encoding.ASCII.GetBytes(_configuration["AES:Key"]);

            var decryptor = aes.CreateDecryptor(aes.Key, iv);

            string password;
            using (var msDecrypt = new MemoryStream(encryptedPassword))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(csDecrypt))
                    {
                        password = sr.ReadToEnd();
                    }
                }
            }

            return password;
        }
    }
}