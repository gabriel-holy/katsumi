using Microsoft.EntityFrameworkCore;
using KatsumiApp.V1.Application.Models;

namespace KatsumiApp.V1.Data.Contexts
{
    public class FollowingContext : DbContext
    {
        public FollowingContext(DbContextOptions<FollowingContext> options) : base(options) { }

        public DbSet<Following> Followings { get; set; }
    }
}
