using Application.Entities;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(int id);

        public Task CreateUserAsync(User user);

        public Task UpdateUserAsync(User user);
    }
}