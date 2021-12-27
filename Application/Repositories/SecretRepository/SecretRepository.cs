using Application.Entities;
using Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.SecretRepository
{
    public class SecretRepository : ISecretRepository
    {
        private readonly IApplicationDbContext _context;

        public SecretRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetServiceNames()
        {
            var serviceNames = await _context.Secrets.Select(s => s.ServiceName).ToListAsync();
            return serviceNames;
        }

        public async Task<string> GetPasswordByServiceName(string serviceName)
        {
            var secret = await _context.Secrets.FirstOrDefaultAsync(s => s.ServiceName == serviceName);
            var password = Encoding.ASCII.GetString(secret.Password);
            return password;
        }

        public async Task CreateSecret(Secret secret)
        {
            await _context.Secrets.AddAsync(secret);
            await _context.SaveChangesAsync();
        }
    }
}