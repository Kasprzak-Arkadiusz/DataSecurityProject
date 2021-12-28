using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibrary.Entities;
using ApiLibrary.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiLibrary.Repositories.SecretRepository
{
    public class SecretRepository : ISecretRepository
    {
        private readonly IApplicationDbContext _context;

        public SecretRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetUserServiceNamesAsync(string userName)
        {
            // Maybe include is needed?
            var serviceNames = await _context.Secrets.Where(s => s.User.UserName == userName)
                .Select(s => s.ServiceName).ToListAsync();
            return serviceNames;
        }

        public async Task<Secret> GetSecretAsync(string serviceName, string userName)
        {
            var secret = await _context.Secrets.Where(s => s.User.UserName == userName)
                .FirstOrDefaultAsync(s => s.ServiceName == serviceName);
            return secret;
        }

        public async Task CreateSecretAsync(Secret secret)
        {
            await _context.Secrets.AddAsync(secret);
            await _context.SaveChangesAsync();
        }
    }
}