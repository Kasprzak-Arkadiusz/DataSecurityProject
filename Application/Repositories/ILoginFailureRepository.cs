using Application.Entities;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ILoginFailureRepository
    {
        public Task<LoginFailure> GetLoginFailureByIdAsync(int id);

        public Task<LoginFailure> CreateLoginFailureAsync(LoginFailure loginFailure);

        public Task UpdateLoginFailureAsync(LoginFailure loginFailure);
    }
}