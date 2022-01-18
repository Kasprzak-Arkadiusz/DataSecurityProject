using ApiLibrary.Common;
using System.Text.RegularExpressions;

namespace ApiLibrary.Validators
{
    internal class MasterPasswordValidator
    {
        internal static bool Validate(string masterPassword)
        {
            if (string.IsNullOrEmpty(masterPassword))
                return false;

            if (masterPassword.Length > Constants.MaxMasterPasswordLength)
                return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            var result = regex.Match(masterPassword);

            return result.Success;
        }
    }
}