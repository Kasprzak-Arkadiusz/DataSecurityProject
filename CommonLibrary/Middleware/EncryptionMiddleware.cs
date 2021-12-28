using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CommonLibrary.Middleware
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;

        public EncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Body = EncryptStream(httpContext.Response.Body);
            httpContext.Request.Body = DecryptStream(httpContext.Request.Body);
            if (httpContext.Request.QueryString.HasValue)
            {
                var decryptedString = DecryptString(httpContext.Request.QueryString.Value?[1..]);
                httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
            }
            await _next(httpContext);
            await httpContext.Request.Body.DisposeAsync();
            await httpContext.Response.Body.DisposeAsync();
        }

        private static CryptoStream EncryptStream(Stream responseStream)
        {
            var aes = GetEncryptionAlgorithm();

            var base64Transform = new ToBase64Transform();
            var base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);

            return cryptoStream;
        }

        private static Aes GetEncryptionAlgorithm()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var aes = Aes.Create();
            aes.Key = Encoding.ASCII.GetBytes(configuration["EncryptQuery:Key"]);
            aes.IV = Encoding.ASCII.GetBytes(configuration["EncryptQuery:Iv"]);

            return aes;
        }

        private static Stream DecryptStream(Stream cipherStream)
        {
            var aes = GetEncryptionAlgorithm();

            var base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            var base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }

        private static string DecryptString(string cipherText)
        {
            var aes = GetEncryptionAlgorithm();
            var buffer = Convert.FromBase64String(cipherText);

            using var memoryStream = new MemoryStream(buffer);
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}