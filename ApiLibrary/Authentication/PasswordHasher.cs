using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ApiLibrary.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        private const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
        private const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
        private const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

        private readonly RNGCryptoServiceProvider _rng;

        public PasswordHasher()
        {
            _rng = new RNGCryptoServiceProvider();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        public byte[] HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var salt = new byte[SaltSize];
            _rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            var outputBytes = new byte[SaltSize + Pbkdf2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 0, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, SaltSize, Pbkdf2SubkeyLength);

            return outputBytes;
        }

        public bool VerifyHashedPassword(byte[] hashedPassword, string providedPassword)
        {
            if (hashedPassword is null || hashedPassword.Length == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(providedPassword))
            {
                return false;
            }

            return VerifyHashedPasswordPrivate(hashedPassword, providedPassword);
        }

        private static bool VerifyHashedPasswordPrivate(byte[] hashedPassword, string password)
        {
            if (hashedPassword.Length != SaltSize + Pbkdf2SubkeyLength)
            {
                return false;
            }

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);

            var expectedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPassword, salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            var actualSubkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            return ByteArraysEqual(actualSubkey, expectedSubkey);
        }
    }
}