using ApiLibrary.Common;
using System.Text.RegularExpressions;

namespace ApiLibrary.Validators
{
    public static class UsernameValidator
    {
        public static bool Validate(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            if (userName.Length > Constants.MaxUsernameLength)
                return false;

            var regex = new Regex(@"^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
            return regex.IsMatch(userName);
        }
    }
}