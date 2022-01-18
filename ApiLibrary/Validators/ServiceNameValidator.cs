using System.Text.RegularExpressions;

namespace ApiLibrary.Validators
{
    public static class ServiceNameValidator
    {
        public static bool Validate(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
                return false;

            var regex = new Regex(@"^[a-zA-Z\d!@#$%^&]+$");
            var result = regex.Match(serviceName);

            return result.Success;
        }
    }
}