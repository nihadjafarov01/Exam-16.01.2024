using Exam2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam2.Contexts
{
    public class Exam2DbContext : IdentityDbContext
    {
        public Exam2DbContext(DbContextOptions options) : base(options) { }

        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
