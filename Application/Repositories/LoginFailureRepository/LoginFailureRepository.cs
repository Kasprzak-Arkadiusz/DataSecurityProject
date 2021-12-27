using System.Threading.Tasks;
using Application.Entities;
using Application.Persistence;

namespace Application.Repositories.LoginFailureRepository
{
    public class LoginFailureRepository : ILoginFailureRepository
    {
        private readonly IApplicationDbContext _context;
        public LoginFailureRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoginFailure> GetLoginFailureByIdAsync(int id)
        {
           var loginFailure = await _context.LoginFailures.FindAsync(id);
           return loginFailure;
        }

        public async Task<LoginFailure> CreateLoginFailureAsync(LoginFailure loginFailure)
        {
            await _context.LoginFailures.AddAsync(loginFailure);
            await _context.SaveChangesAsync();
            return loginFailure;
        }

        public async Task UpdateLoginFailureAsync(LoginFailure loginFailure)
        {
            var loginFailureToUpdate = await _context.LoginFailures.FindAsync(loginFailure.Id);

            loginFailureToUpdate.NumberOfFailedAttempts = loginFailure.NumberOfFailedAttempts;
            loginFailureToUpdate.IsTemporaryLockout = loginFailure.IsTemporaryLockout;
            loginFailureToUpdate.LockoutTo = loginFailure.LockoutTo;
            loginFailureToUpdate.NumberOfLockoutsInARow = loginFailure.NumberOfLockoutsInARow;
            loginFailureToUpdate.User = loginFailure.User;

            await _context.SaveChangesAsync();
        }
    }
}