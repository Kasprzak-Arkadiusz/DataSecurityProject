using Application.Entities;
using Application.Persistence;
using System.Threading.Tasks;

namespace Application.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _context;

        public UserRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var userToUpdate = await _context.Users.FindAsync(user.Id);

            userToUpdate.UserName = user.UserName;
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;
            userToUpdate.MasterPassword = user.MasterPassword;

            await _context.SaveChangesAsync();
        }
    }
}