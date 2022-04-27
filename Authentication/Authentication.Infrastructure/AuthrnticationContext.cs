using Authentication.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure
{
    public class AuthrnticationContext : DbContext
    {
        public AuthrnticationContext(DbContextOptions<AuthrnticationContext> options)
       : base(options)
        {
        }
        public DbSet<UserAuthentication> UserAuthentications => Set<UserAuthentication>();
    }
}