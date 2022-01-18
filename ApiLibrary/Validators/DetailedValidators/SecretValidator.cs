using CommonLibrary.Dto;

namespace ApiLibrary.Validators.DetailedValidators
{
    public static class SecretValidator
    {
        public static bool Validate(SecretDto dto)
        {
            if (dto is null)
                return false;

            if (!ServiceNameValidator.Validate(dto.ServiceName))
                return false;

            if (!ServicePasswordValidator.Validate(dto.Password))
                return false;

            return true;
        }
    }
}