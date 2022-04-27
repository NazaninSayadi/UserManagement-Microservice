using User.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace User.Infrastructure
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
       : base(options)
        {
        }
        public DbSet<UserEntity> Users => Set<UserEntity>();
    }
}