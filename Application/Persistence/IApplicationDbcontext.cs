using Application.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Persistence
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync();

        public DbSet<User> Users { get; set; }
        public DbSet<LastConnection> LastConnections { get; set; }
        public DbSet<LoginFailure> LoginFailures { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Secret> Secrets { get; set; }
    }
}