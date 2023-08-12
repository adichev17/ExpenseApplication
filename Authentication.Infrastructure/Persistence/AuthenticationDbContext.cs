using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence
{
    public class AuthenticationDbContext : DbContext, IAuthenticationDbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //Test Data
           modelBuilder.Entity<UserEntity>().HasData(
               new UserEntity[]
               {
                   new UserEntity{ Id = Guid.NewGuid(), Login = "Admin", Password = "$2a$11$M9/3ctN64Xo8XnntPCcrhetHfzu2.AaNRHwCdjOsWxwpNzU8khNWq", CreatedOnUtc = DateTime.Now } // Admin1$1
               });
        }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
