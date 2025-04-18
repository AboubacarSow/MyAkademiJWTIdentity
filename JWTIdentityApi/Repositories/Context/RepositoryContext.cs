using JWTIdentityApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTIdentityApi.Repositories.Context
{
    public class RepositoryContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

      
    }
}
