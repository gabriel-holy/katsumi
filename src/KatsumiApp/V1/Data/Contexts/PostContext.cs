using KatsumiApp.V1.Application.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace KatsumiApp.V1.Data.Contexts
{
    public class PostContext
    {
        public class QuotePostContext : DbContext
        {
            public QuotePostContext(DbContextOptions<QuotePostContext> options) : base(options) { }

            public DbSet<QuotePost> QuotePosts { get; set; }
        }

        public class RegularPostContext : DbContext
        {
            public RegularPostContext(DbContextOptions<RegularPostContext> options) : base(options) { }

            public DbSet<RegularPost> RegularPosts { get; set; }
        }

        public class SharedPostContext : DbContext
        {
            public SharedPostContext(DbContextOptions<SharedPostContext> options) : base(options) { }

            public DbSet<SharedPost> SharedPosts { get; set; }
        }
    }
}
