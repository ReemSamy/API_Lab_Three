using Lab_Three_.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab_Three_.Data.Context
{
    public class UsersContext : IdentityDbContext<Employee>
    {
        public UsersContext(DbContextOptions<UsersContext> options) :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().ToTable(nameof(Users));
            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
        }
    }
}
