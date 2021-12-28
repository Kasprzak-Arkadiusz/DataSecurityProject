using System.Threading.Tasks;
using ApiLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiLibrary.Persistence
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