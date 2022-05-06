using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using KatsumiApp.V1.Application.Models.Post;
using System.Collections.Generic;
using System.Linq;
using KatsumiApp.V1.Data.Raven.Contexts;

namespace KatsumiApp.V1.Application.Features.Post.Regular.UseCases
{
    public class MakeRegularPost
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {

                var post = command.MapToDomain();

                using var databaseSession = PostContext.RegularPostContext.DocumentStore.OpenAsyncSession();

                await databaseSession.StoreAsync(post, cancellationToken);

                await databaseSession.SaveChangesAsync(cancellationToken);

                var result = post.MapFromDomain();

                return result;
            }
        }

        public class Command : IRequest<Result>
        {
            public string Username { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string Media { get; set; }
            public IEnumerable<string> Keywords { get; set; }
        }

        public class Result
        {
            public string Id { get; set; }
        }
    }

    public static class RegularPostMapper
    {
        public static MakeRegularPost.Result MapFromDomain(this RegularPost regularPost)
        {
            return new MakeRegularPost.Result()
            {
                Id = regularPost.Id,
            };
        }

        public static RegularPost MapToDomain(this MakeRegularPost.Command command)
        {
            return new RegularPost()
            {
                Username = command.Username,
                CreatedAtUtc = DateTime.UtcNow,
                PostContent = new RegularPost.Content()
                {
                    Title = command.Title,
                    Keywords = new List<string>(command.Keywords.Select(x => x)),
                    Text = command.Text,
                    Media = command.Media,
                }
            };
        }
    }
}
