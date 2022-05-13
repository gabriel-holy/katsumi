using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using KatsumiApp.V1.Data.Raven.Contexts;
using KatsumiApp.V1.Application.Features.UserProfile.DomainEvents;
using KatsumiApp.V1.Application.Domain;

namespace KatsumiApp.V1.Application.Features.UserProfile.UseCases
{
    public class ListUserProfile
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            public Handler(IMapper mapper, IMediator mediator)
            {
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                using var databaseSession = UserProfileContext.DocumentStore.OpenAsyncSession();

                var userProfile = (await databaseSession.Query<KatsumiApp.V1.Application.Domain.UserProfile>()
                                                        .FirstOrDefaultAsync(user => user.Username == command.Username, cancellationToken))
                                                        .MapFromDomain();

                if (userProfile == null)
                {
                    return null;
                }

                await BuildUserProfileComplementaryData(userProfile, command.ViewerUsername);

                var result = userProfile;

                await _mediator.Send(new UserProfileCreatedEvent.Command(), cancellationToken);

                return result;
            }

            private async Task BuildUserProfileComplementaryData(Result userProfileToBuild, string viewerUserName)
            {
                using var followingDatabaseSession = FollowingContext.DocumentStore.OpenAsyncSession();

                var fetchTotalFollowersTask = FetchTotalFollowersByUsername(userProfileToBuild.Username, followingDatabaseSession);
                var fetchTotalFollowingTask = FetchTotalFollowingByUsername(userProfileToBuild.Username, followingDatabaseSession);
                var fetchFollowingBetweenUsersTask = FetchFollowingBetweenUsers(viewerUserName, userProfileToBuild.Username, followingDatabaseSession);
                var fetchUserFeed = FetchUserFeed(userProfileToBuild.Username, followingDatabaseSession);

                await Task.WhenAll(fetchTotalFollowersTask, fetchTotalFollowingTask, fetchFollowingBetweenUsersTask, fetchUserFeed);

                userProfileToBuild.TotalFollowers = await fetchTotalFollowersTask;
                userProfileToBuild.TotalFollowing = await fetchTotalFollowingTask;
                userProfileToBuild.AreViewerFollowingThisUser = await fetchFollowingBetweenUsersTask;
                userProfileToBuild.Feed = await fetchUserFeed;
            }

            private async Task<Result.UserFeed> FetchUserFeed(string username, IAsyncDocumentSession databaseSession)
            {
                var regularPosts = FetchRegularPostsByUsername(username, databaseSession);
                var sharedPosts = FetchSharedPostsByUsername(username, databaseSession);
                var quotePosts = FetchQuotePostsByUsername(username, databaseSession);

                await Task.WhenAll(regularPosts, sharedPosts, quotePosts);

                var finalPostList = ApplyOrdenationAndSizingOnFeed(regularPosts, sharedPosts, quotePosts);

                Result.UserFeed userFeed = new();

                userFeed.LastFewPosts = new List<object>() { await finalPostList };

                return userFeed;
            }


            private async Task<IEnumerable<RegularPost>> FetchRegularPostsByUsername(string username, IAsyncDocumentSession databaseSession)
                => null; /* await _regularPostContext
                        .RegularPosts
                            .Include(i => i.PostContent)
                                .ThenInclude(i => i.Keywords)

                        .Where(post => post.Username == username)
                        .OrderByDescending(post => post.CreatedAtUtc)
                        .Take(5)
                        .ToListAsync();
                */
            private async Task<IEnumerable<SharedPost>> FetchSharedPostsByUsername(string username, IAsyncDocumentSession databaseSession)
                 => null; /* await _sharedPostContext

                         .SharedPosts
                             .Include(i => i.OriginalPost)
                                 .ThenInclude(i => i.PostContent)
                                     .ThenInclude(i => i.Keywords)

                         .Where(post => post.Username == username)
                         .OrderByDescending(post => post.CreatedAtUtc)
                         .Take(5)
                         .ToListAsync();
            */

            private async Task<IEnumerable<QuotePost>> FetchQuotePostsByUsername(string username, IAsyncDocumentSession databaseSession)
            {
                return await databaseSession
                            .Query<QuotePost>()
                            .Where(post => post.Username == username)
                            .OrderByDescending(post => post.CreatedAtUtc)
                            .Take(5)
                            .ToListAsync();
            }

            private static async Task<int> FetchTotalFollowersByUsername(string username, IAsyncDocumentSession databaseSession)
            {
                return await databaseSession
                             .Query<KatsumiApp.V1.Application.Domain.Following>()
                             .Where(following => following.FollowingIsActive && following.FollowedUsername == username)
                             .CountAsync();
            }
            private static async Task<int> FetchTotalFollowingByUsername(string username, IAsyncDocumentSession databaseSession)
            {
                return await databaseSession
                             .Query<KatsumiApp.V1.Application.Domain.Following>()
                             .Where(following => following.FollowingIsActive && following.FollowerUsername == username)
                             .CountAsync();
            }
            private static async Task<bool> FetchFollowingBetweenUsers(string viewerUsername, string username, IAsyncDocumentSession databaseSession)
            {
                return await databaseSession
                            .Query<KatsumiApp.V1.Application.Domain.Following>()
                            .AnyAsync(following => following.FollowingIsActive &&
                                        following.FollowerUsername == viewerUsername &&
                                        following.FollowedUsername == username);
            }
            private static async Task<IList<object>> ApplyOrdenationAndSizingOnFeed(
              Task<IEnumerable<RegularPost>> regularPosts,
              Task<IEnumerable<SharedPost>> sharedPosts,
              Task<IEnumerable<QuotePost>> quotePosts)
            {
                const int FeedUserProfileLimitThatShouldBeMigratedToAHotConfiguration = 5;

                var postList = new List<dynamic>();

                postList.AddRange(await regularPosts);
                postList.AddRange(await sharedPosts);
                postList.AddRange(await quotePosts);

                postList = postList.OrderByDescending(post => post.CreatedAtUtc).ToList();

                return postList.Take(FeedUserProfileLimitThatShouldBeMigratedToAHotConfiguration).ToList();
            }
        }

        public class Command : IRequest<Result>
        {
            public string Username { get; set; }
            public string ViewerUsername { get; set; }
        }

        public class Result
        {
            public Result()
            {
                if (Feed == null) Feed = new UserFeed();
            }
            public string Id { get; set; }
            public string Username { get; set; }
            public DateTime JoinedSocialMediaAt { get; set; }
            public string JoinedSocialMediaAtFormated => "Katsumi's member since " + JoinedSocialMediaAt.ToString("MMMM dd, yyyy");
            public int TotalFollowing { get; set; }
            public int TotalFollowers { get; set; }
            public bool AreViewerFollowingThisUser { get; set; }
            public UserFeed Feed { get; set; }

            public class UserFeed
            {
                public UserFeed()
                {
                    if (LastFewPosts == null) LastFewPosts = new List<object>();
                }
                public DateTime FetchAtUtc { get; private set; } = DateTime.UtcNow;
                public List<object> LastFewPosts { get; set; }
            }
        }
    }

    public static class Mapper
    {
        public static ListUserProfile.Result MapFromDomain(this KatsumiApp.V1.Application.Domain.UserProfile userProfile)
        {
            var result = new ListUserProfile.Result()
            {
                Username = userProfile.Username,
                Id = userProfile.Id,
                JoinedSocialMediaAt = userProfile.JoinedSocialMediaAt,
            };

            return result;
        }
    }
}