using CommonLibrary.Dto;

namespace ApiLibrary.Validators.DetailedValidators
{
    public static class UpdatePasswordValidator
    {
        public static bool Validate(UpdatePasswordDto dto)
        {
            if (dto == null)
                return false;

            if (!PasswordValidator.Validate(dto.Password))
                return false;

            if (!EmailValidator.Validate(dto.Email))
                return false;

            return true;
        }
    }
}