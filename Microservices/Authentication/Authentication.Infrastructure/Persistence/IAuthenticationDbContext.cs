using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence
{
    public interface IAuthenticationDbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        Task<int> SaveChangesAsync();
    }
}
