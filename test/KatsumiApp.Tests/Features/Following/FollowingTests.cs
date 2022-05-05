using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using KatsumiApp.V1.Application.Features.Following.Follow.UseCases;
using KatsumiApp.V1.Application.Features.Following.Unfollow.UseCases;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.Tests.Features.Following
{
    public class FollowingTests : BaseIntegrationTest
    {
        public class FollowTest : FollowingTests
        {

            [Fact(DisplayName = "Follow with success and find the insertion on database.")]
            public async Task Follow_with_success_and_find_the_insertion_on_database()
            {
                // Arrange
                const string followed = "adam";
                const string follower = "gabriel";
                var serviceProvider = _services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var result = await mediator.Send(new FollowUserProfile.Command
                {
                    FollowedUsername = followed,
                    FollowerUsername = follower
                });

                // Assert
                using var assertScope = serviceProvider.CreateScope();

                var db = assertScope.ServiceProvider.GetRequiredService<FollowingContext>();

                var followingResult = await db.Followings
                                              .Where(f => f.FollowedUsername == followed &&
                                                     f.FollowerUsername == follower)
                                              .FirstOrDefaultAsync();

                Assert.True(result.FollowingIsActive);
                Assert.NotNull(followingResult);
                Assert.Equal(follower, followingResult.FollowerUsername);
                Assert.Equal(followed, followingResult.FollowedUsername);
            }

            [Fact(DisplayName = "Follow yourself and receive ValidationException.")]
            public async Task Follow_yourself_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var follower = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new FollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = follower
                })); ;
            }

            [Fact(DisplayName = "Exceed follower username length and receive ValidationException.")]
            public async Task Exceed_follower_username_length_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var tooBigUsername = "kingHenryFiveTheWarrior";
                var followed = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new FollowUserProfile.Command
                {
                    FollowerUsername = tooBigUsername,
                    FollowedUsername = followed
                })); ;
            }

            [Fact(DisplayName = "Exceed followed username length and receive ValidationException.")]
            public async Task Exceed_followed_username_length_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var tooBigUsername = "kingHenryFiveTheWarrior";
                var follower = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new FollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = tooBigUsername
                })); ;
            }

            [Fact(DisplayName = "Using special characters on followedUsername and receive ValidationException.")]
            public async Task Using_special_characters_on_followedUsername_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var specialCharactersUsername = "adam.beyer";
                var follower = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new FollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = specialCharactersUsername
                })); ;
            }

            [Fact(DisplayName = "Using special characters on followerUsername and receive ValidationException.")]
            public async Task Using_special_characters_on_followerUsername_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var specialCharactersUsername = "gabriel.oliveira";
                var follower = "adam";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new FollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = specialCharactersUsername
                })); ;
            }
        }

        public class UnfollowTest : FollowingTests
        {
            [Fact(DisplayName = "Unfollow with success and find the insertion on database.")]
            public async Task Unfollow_with_success_and_find_the_insertion_on_database()
            {
                // Arrange
                const string followed = "gabriel";
                const string follower = "steve";
                var serviceProvider = _services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var result = await mediator.Send(new UnfollowUserProfile.Command
                {
                    FollowedUsername = followed,
                    FollowerUsername = follower
                });

                // Assert
                using var assertScope = serviceProvider.CreateScope();

                var db = assertScope.ServiceProvider.GetRequiredService<FollowingContext>();

                var followingResult = await db.Followings
                                              .Where(f => f.FollowedUsername == followed &&
                                                     f.FollowerUsername == follower)
                                              .FirstOrDefaultAsync();

                Assert.False(result.FollowingIsActive);
                Assert.NotNull(followingResult);
                Assert.Equal(follower, followingResult.FollowerUsername);
                Assert.Equal(followed, followingResult.FollowedUsername);
            }

            [Fact(DisplayName = "Try to unfollow an unexisting following.")]
            public async Task Try_to_unfollow_an_unexisting_following()
            {
                // Arrange
                const string followed = "unexistentUser";
                const string follower = "steve";
                var serviceProvider = _services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var result = await mediator.Send(new UnfollowUserProfile.Command
                {
                    FollowedUsername = followed,
                    FollowerUsername = follower
                });

                // Assert
                using var assertScope = serviceProvider.CreateScope();

                var db = assertScope.ServiceProvider.GetRequiredService<FollowingContext>();

                var followingResult = await db.Followings
                                              .Where(f => f.FollowedUsername == followed &&
                                                     f.FollowerUsername == follower)
                                              .FirstOrDefaultAsync();

                Assert.Null(result);
                Assert.Null(followingResult);
            }

            [Fact(DisplayName = "Exceed follower username length and receive ValidationException.")]

            public async Task Exceed_follower_username_length_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var tooBigUsername = "kingHenryFiveTheWarrior";
                var followed = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new UnfollowUserProfile.Command
                {
                    FollowerUsername = tooBigUsername,
                    FollowedUsername = followed
                })); ;
            }

            [Fact(DisplayName = "Exceed followed username length and receive ValidationException.")]
            public async Task Exceed_followed_username_length_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var tooBigUsername = "kingHenryFiveTheWarrior";
                var follower = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new UnfollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = tooBigUsername
                })); ;
            }

            [Fact(DisplayName = "Using special characters on followedUsername and receive ValidationException.")]
            public async Task Using_special_characters_on_followedUsername_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var specialCharactersUsername = "adam.beyer";
                var follower = "gabriel";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new UnfollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = specialCharactersUsername
                })); ;
            }

            [Fact(DisplayName = "Using special characters on followerUsername and receive ValidationException.")]
            public async Task Using_special_characters_on_followerUsername_and_receive_ValidationException()
            {
                // Arrange
                using var scope = _services.BuildServiceProvider().CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var specialCharactersUsername = "gabriel.oliveira";
                var follower = "adam";

                // Act & Assert
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => mediator.Send(new UnfollowUserProfile.Command
                {
                    FollowerUsername = follower,
                    FollowedUsername = specialCharactersUsername
                })); ;
            }
        }


    }
}
