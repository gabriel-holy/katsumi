using ForEvolve.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.V1.Data.EntityFramework
{
    public class DependencyInjectionDataModule : DependencyInjectionModule
    {
        public DependencyInjectionDataModule(IServiceCollection services) : base(services)
        {
            services.AddDbContext<FollowingContext>(options => options
                .UseInMemoryDatabase(nameof(FollowingContext))
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );

            services.AddDbContext<UserProfileContext>(options => options
                .UseInMemoryDatabase(nameof(UserProfileContext))
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );

            services.AddDbContext<PostContext.RegularPostContext>(options => options
                .UseInMemoryDatabase(nameof(PostContext.RegularPostContext))
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );

            services.AddDbContext<PostContext.QuotePostContext>(options => options
                .UseInMemoryDatabase(nameof(PostContext.QuotePostContext))
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );

            services.AddDbContext<PostContext.SharedPostContext>(options => options
                .UseInMemoryDatabase(nameof(PostContext.SharedPostContext))
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );

            services.AddForEvolveSeeders().Scan<Startup>();
        }
    }
}
