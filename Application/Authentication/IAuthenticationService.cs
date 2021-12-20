using System.Threading.Tasks;
using Application.Common;
using Application.Common.Dto;

namespace Application.Authentication
{
    public interface IAuthenticationService
    {
        public Task<Result> RegisterAsync(UserDto user);
        public bool Login(string userName, string password);
        public void Logout(string userName);
    }
}