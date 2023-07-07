using Microsoft.EntityFrameworkCore;
using Congratulator.Models;

namespace Congratulator.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    }
}