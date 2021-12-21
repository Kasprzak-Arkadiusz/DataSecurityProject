using System.Threading.Tasks;
using Application.Common;
using Application.Common.Dto;

namespace Application.Authentication
{
    public interface IAuthenticationService
    {
        public Task<Result> RegisterAsync(RegisterDto user);
        public Task<LoginResponse> LoginAsync(LoginDto login);
        public void Logout(string userName);
    }
}