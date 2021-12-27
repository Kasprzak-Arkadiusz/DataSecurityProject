using Application.Entities;
using System.Threading.Tasks;

namespace Application.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(int id);

        public Task<User> GetUserByNameAsync(string username);

        public Task<User> GetUserByEmailAsync(string email);

        public Task CreateUserAsync(User user);

        public Task UpdateUserAsync(User user);
    }
}