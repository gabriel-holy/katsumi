using Microsoft.EntityFrameworkCore;
using KatsumiApp.V1.Application.Models;

namespace KatsumiApp.V1.Data.Contexts
{
    public class UserProfileContext : DbContext
    {
        public UserProfileContext(DbContextOptions<UserProfileContext> options) : base(options) { }

        public DbSet<UserProfile> UsersProfiles { get; set; }
    }
}
