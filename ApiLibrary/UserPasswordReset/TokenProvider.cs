﻿using ApiLibrary.Common;
using ApiLibrary.Entities;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiLibrary.UserPasswordReset
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IDataProtector _protector;

        public TokenProvider(IDataProtectionProvider provider)
        {
            var purpose = Environment.GetEnvironmentVariable("PurposeForTokenProviderProtector");
            _protector = provider.CreateProtector(purpose);
        }

        public async Task<string> GenerateAsync(User user)
        {
            var ms = new MemoryStream();

            await using (var writer = ms.CreateWriter())
            {
                writer.Write(DateTimeOffset.Now);
                writer.Write(user.Id);
            }

            var token = ms.ToArray();
            var protectedBytes = _protector.Protect(token);
            return Convert.ToBase64String(protectedBytes);
        }

        public bool Validate(User user, string token)
        {
            try
            {
                var unprotectedData = _protector.Unprotect(Convert.FromBase64String(token));
                var ms = new MemoryStream(unprotectedData);
                using var reader = ms.CreateReader();

                var creationTime = reader.ReadDateTimeOffset();
                var expirationTime = creationTime + TimeSpan.FromMinutes(Constants.ResetPasswordExpirationTimeInMinutes);
                if (expirationTime < DateTimeOffset.Now)
                {
                    return false;
                }

                var userId = reader.ReadInt32();
                return userId == user.Id;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}