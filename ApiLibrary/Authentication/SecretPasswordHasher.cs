using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ApiLibrary.Authentication
{
    public class SecretPasswordHasher : ISecretPasswordHasher
    {
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
            var aesKeyString = Environment.GetEnvironmentVariable("AESKEY");
            aes.Key = Encoding.ASCII.GetBytes(aesKeyString);

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
            var aesKeyString = Environment.GetEnvironmentVariable("AESKEY");
            aes.Key = Encoding.ASCII.GetBytes(aesKeyString);

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