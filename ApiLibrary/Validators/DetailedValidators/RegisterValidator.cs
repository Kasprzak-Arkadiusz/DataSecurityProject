using CommonLibrary.Dto;

namespace ApiLibrary.Validators.DetailedValidators
{
    public static class RegisterValidator
    {
        public static bool Validate(RegisterDto dto)
        {
            if (dto == null)
                return false;

            if (!UsernameValidator.Validate(dto.UserName))
                return false;

            if (!EmailValidator.Validate(dto.Email))
                return false;

            if (!PasswordValidator.Validate(dto.Password))
                return false;

            if (!MasterPasswordValidator.Validate(dto.MasterPassword))
                return false;

            if (dto.Password == dto.MasterPassword)
                return false;

            return true;
        }
    }
}