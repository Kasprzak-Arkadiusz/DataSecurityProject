using System.Reflection;
using System.Threading.Tasks;
using ApiLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiLibrary.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext()
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<LastConnection> LastConnections { get; set; }
        public DbSet<LoginFailure> LoginFailures { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Secret> Secrets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}