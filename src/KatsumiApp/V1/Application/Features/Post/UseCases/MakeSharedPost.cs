using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using KatsumiApp.V1.Data.Raven.Contexts;
using KatsumiApp.V1.Application.Domain;

namespace KatsumiApp.V1.Application.Features.Post.UseCases
{
    public class MakeSharedPost
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                using var regularPostDatabaseSession = PostContext.RegularPostContext.DocumentStore.OpenAsyncSession();

                var originalPost = await regularPostDatabaseSession.Query<RegularPost>().FirstOrDefaultAsync(p => p.Id == command.OriginalPostId, cancellationToken);

                if (originalPost == null)
                {
                    throw new Exception($"Original post {command.OriginalPostId} not found to be shared or quoted.");
                }

                using var sharedPostDatabaseSession = PostContext.SharedPostContext.DocumentStore.OpenAsyncSession();

                var sharing = command.MapToDomain();

                await sharedPostDatabaseSession.StoreAsync(sharing, cancellationToken);

                await sharedPostDatabaseSession.SaveChangesAsync(cancellationToken);

                var result = sharing.MapFromDomain();

                return result;
            }
        }

        public class Command : IRequest<Result>
        {
            public string OriginalPostId { get; set; }
            public string Username { get; set; }
        }

        public class Result
        {
            public string Id { get; set; }
        }
    }

    public static class SharedPostMapper
    {
        public static MakeSharedPost.Result MapFromDomain(this SharedPost sharedPost)
        {
            return new MakeSharedPost.Result()
            {
                Id = sharedPost.Id,
            };
        }

        public static SharedPost MapToDomain(this MakeSharedPost.Command command)
        {
            return new SharedPost()
            {
                Username = command.Username,
                CreatedAtUtc = DateTime.UtcNow,
                OriginalPostId = command.OriginalPostId,
            };
        }
    }
}
