using System.Threading.Tasks;
using Application.Entities;

namespace Application.Repositories.LoginFailureRepository
{
    public interface ILoginFailureRepository
    {
        public Task<LoginFailure> GetLoginFailureByIdAsync(int id);

        public Task<LoginFailure> CreateLoginFailureAsync(LoginFailure loginFailure);

        public Task UpdateLoginFailureAsync(LoginFailure loginFailure);
    }
}