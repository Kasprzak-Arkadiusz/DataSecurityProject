﻿using System.Runtime.CompilerServices;

namespace ApiLibrary.Common
{
    public static class Constants
    {
        public const int MaxNumberOfFailedAttempts = 3;
        public const int JwtExpirationTimeInMinutes = 60;
        public const int NumberOfLastConnections = 5;
        public const int ResetPasswordExpirationTimeInMinutes = 30;
        public static readonly int[] LockoutSpansInMinutes = {5, 30};
        public static readonly int MaxNumberOfLockouts = LockoutSpansInMinutes.Length + 1;

        public const int MaxPasswordLength = 128;
        public const int MaxMasterPasswordLength = 128;
        public const int MaxUsernameLength = 32;
    }
}