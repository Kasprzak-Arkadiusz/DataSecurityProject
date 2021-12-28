using CommonLibrary.Common;
using CommonLibrary.Dto;
using System.Threading.Tasks;

namespace ApiLibrary.Authentication
{
    public interface IAuthenticationService
    {
        public Task<Result> RegisterAsync(RegisterDto user);

        public Task<LoginResponse> LoginAsync(LoginDto login, string jwtKey);
    }
}