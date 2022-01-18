using System.Text.RegularExpressions;

namespace ApiLibrary.Validators
{
    public static class ServicePasswordValidator
    {
        public static bool Validate(string servicePassword)
        {
            if (string.IsNullOrEmpty(servicePassword))
                return false;

            var regex = new Regex(@"^[a-zA-Z\d!@#$%^&]+$");
            var result = regex.Match(servicePassword);

            return result.Success;
        }
    }
}