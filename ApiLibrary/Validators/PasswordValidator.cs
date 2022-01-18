using ApiLibrary.Common;
using System.Text.RegularExpressions;

namespace ApiLibrary.Validators
{
    internal static class PasswordValidator
    {
        internal static bool Validate(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            if (password.Length > Constants.MaxPasswordLength)
                return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            var result = regex.Match(password);

            return result.Success;
        }
    }
}