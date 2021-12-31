using System;
using ApiLibrary.Entities;
using ApiLibrary.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibrary.Repositories.LastConnectionRepository
{
    public class LastConnectionRepository : ILastConnectionRepository
    {
        private readonly IApplicationDbContext _context;

        public LastConnectionRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LastConnection>> GetByUserIdAsync(int userId, int count)
        {
            try
            {
                var lastConnections = await  _context.LastConnections.Include(l => l.User)
                    .Where(l => l.User.Id == userId)
                    .OrderByDescending(l => l.ConnectionTime)
                    .ToListAsync();

                var result = lastConnections.Take(count);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task CreateAsync(LastConnection connection)
        {
            await _context.LastConnections.AddAsync(connection);
            await _context.SaveChangesAsync();
        }
    }
}