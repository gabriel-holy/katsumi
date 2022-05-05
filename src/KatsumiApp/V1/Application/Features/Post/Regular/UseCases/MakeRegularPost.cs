using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using KatsumiApp.V1.Application.Models.Post;
using System.Collections.Generic;
using System.Linq;
using KatsumiApp.V1.Application.Features.Post.Regular.UseCases;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.V1.Application.Features.Post.Regular.UseCases
{
    public class MakeRegularPost
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PostContext.RegularPostContext _regularPostContext;

            public Handler(PostContext.RegularPostContext regularPostContext, IMapper mapper)
            {
                _regularPostContext = regularPostContext ?? throw new ArgumentNullException(nameof(regularPostContext));
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var post = command.MapToDomain();

                await _regularPostContext.AddAsync(post, cancellationToken);
                await _regularPostContext.SaveChangesAsync(cancellationToken);

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
                    Keywords = new List<RegularPost.Content.Keyword>(command.Keywords.Select(keyword => new RegularPost.Content.Keyword(keyword))),
                    Text = command.Text,
                    Media = command.Media,
                }
            };
        }
    }
}
