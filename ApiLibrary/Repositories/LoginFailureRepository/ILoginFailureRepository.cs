using System.Threading.Tasks;
using ApiLibrary.Entities;

namespace ApiLibrary.Repositories.LoginFailureRepository
{
    public interface ILoginFailureRepository
    {
        public Task<LoginFailure> GetLoginFailureByUserIdAsync(int userId);

        public Task<LoginFailure> CreateLoginFailureAsync(LoginFailure loginFailure);

        public Task UpdateLoginFailureAsync(LoginFailure loginFailure);
    }
}