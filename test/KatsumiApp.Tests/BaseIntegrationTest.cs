using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KatsumiApp.V1.Data.Contexts;
using KatsumiApp.V1.Application.Models;

namespace KatsumiApp.Tests
{
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        protected readonly IServiceCollection _services;
        public BaseIntegrationTest()
        {
            _services = new ServiceCollection();

            new Startup().ConfigureServices(_services);

            RemoveDbContext<FollowingContext>();

            _services.AddDbContext<FollowingContext>(options => options
                .UseInMemoryDatabase(nameof(FollowingContext))
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );

            RemoveDbContext<UserProfileContext>();

            _services.AddDbContext<UserProfileContext>(options => options
              .UseInMemoryDatabase(nameof(UserProfileContext))
              .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
          );
        }

        protected virtual async Task SeedAsync(FollowingContext db)
        {
            await db.Followings.AddAsync(new Following
            {
                FollowedUsername = "gabriel",
                FollowerUsername = "steve",
                FollowingIsActive = true,
            });

            await db.Followings.AddAsync(new Following
            {
                FollowedUsername = "gabriel",
                FollowerUsername = "lucas",
                FollowingIsActive = true,
            });

            await db.Followings.AddAsync(new Following
            {
                FollowedUsername = "lucas",
                FollowerUsername = "gabriel",
                FollowingIsActive = true,
            });

            await db.SaveChangesAsync();
        }

        protected virtual async Task SeedAsync(UserProfileContext db)
        {
            await db.UsersProfiles.AddAsync(new UserProfile
            {
                Username = "adam",
                JoinedSocialMediaAt = new DateTime(2015, 06, 20)
            }); ;

            await db.UsersProfiles.AddAsync(new UserProfile
            {
                Username = "gabriel",
                JoinedSocialMediaAt = new DateTime(2016, 11, 01)
            }); ;

            await db.UsersProfiles.AddAsync(new UserProfile
            {
                Username = "lucas",
                JoinedSocialMediaAt = new DateTime(2020, 03, 10)
            }); ;

            await db.UsersProfiles.AddAsync(new UserProfile
            {
                Username = "steve",
                JoinedSocialMediaAt = new DateTime(2021, 03, 27)
            }); ;

            await db.SaveChangesAsync();
        }

        protected void RemoveDbContext<TDbContext>() where TDbContext : DbContext
        {
            var dbContextDescriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(TDbContext));

            var optionsDescriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<TDbContext>));

            _services.Remove(dbContextDescriptor);

            _services.Remove(optionsDescriptor);
        }

        public async Task InitializeAsync()
        {
            await SeedAsync(_services
                .BuildServiceProvider()
                .CreateScope().ServiceProvider
                .GetRequiredService<FollowingContext>());

            await SeedAsync(_services
               .BuildServiceProvider()
               .CreateScope().ServiceProvider
               .GetRequiredService<UserProfileContext>());
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
