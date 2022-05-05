using System.Threading.Tasks;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Globalization;
using static KatsumiApp.V1.Application.Features.UserProfile.UseCases.ListUserProfile;

namespace KatsumiApp.Tests.Features.Person
{
    public class UserProfileTest : BaseIntegrationTest
    {
        public class ListUserProfile : UserProfileTest
        {
            [Fact(DisplayName = "Fetch an user from database with sucess")]
            public async Task Fetch_an_user_from_database_with_sucess()
            {
                // Arrange
                const string username = "gabriel";
                const string viewer = "steve";

                var serviceProvider = _services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var result = await mediator.Send(new Command()
                {
                    ViewerUsername = viewer,
                    Username = username
                });

                // Assert
                Assert.True(result.TotalFollowers > 0);
                Assert.True(result.TotalFollowing > 0);
                Assert.True(result.AreViewerFollowingThisUser);
                Assert.True(result.Username == username);
            }

            [Fact(DisplayName = "Fetch an user and check join date format")]
            public async Task Fetch_an_user_and_check_join_date_format()
            {
                // Arrange

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                const string username = "adam";
                const string viewer = "gabriel";
                const string formatedDate = "June 20, 2015";

                var serviceProvider = _services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var result = await mediator.Send(new Command()
                {
                    ViewerUsername = viewer,
                    Username = username
                });

                // Assert
                Assert.Contains(formatedDate, result.JoinedSocialMediaAtFormated);
            }
        }
    }
}
