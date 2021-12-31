using ApiLibrary.Entities;
using ApiLibrary.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApiLibrary.Repositories.PasswordResetRepository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly IApplicationDbContext _context;

        public PasswordResetRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordReset> GetByUserId(int userId)
        {
            var passwordReset = await _context.PasswordResets.Include(p => p.User)
                .Where(p => p.User.Id == userId).FirstOrDefaultAsync();
            return passwordReset;
        }

        public async Task CreatePasswordReset(PasswordReset passwordReset)
        {
            await _context.PasswordResets.AddAsync(passwordReset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var passwordReset = await _context.PasswordResets.FindAsync(id);
            _context.PasswordResets.Remove(passwordReset);
            await _context.SaveChangesAsync();
        }
    }
}