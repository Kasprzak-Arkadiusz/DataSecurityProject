using ApiLibrary.Entities;
using ApiLibrary.Persistence;
using System;
using System.Threading.Tasks;

namespace ApiLibrary.Repositories.PasswordResetRepository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly IApplicationDbContext _context;

        public PasswordResetRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordReset> GetPasswordResetById(int id)
        {
            var passwordReset = await _context.PasswordResets.FindAsync(id);
            return passwordReset;
        }

        public async Task CreatePasswordReset(PasswordReset passwordReset)
        {
            try
            {
                await _context.PasswordResets.AddAsync(passwordReset);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}