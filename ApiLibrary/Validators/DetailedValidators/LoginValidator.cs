using CommonLibrary.Dto;

namespace ApiLibrary.Validators.DetailedValidators
{
    public class LoginValidator
    {
        public static bool Validate(LoginDto dto)
        {
            if (dto == null)
                return false;

            if (!UsernameValidator.Validate(dto.UserName))
                return false;

            if (!PasswordValidator.Validate(dto.Password))
                return false;

            return true;
        }
    }
}
