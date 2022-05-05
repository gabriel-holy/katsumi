using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using KatsumiApp.V1.Application.Models.Post;
using KatsumiApp.V1.Application.Features.Post.Shared.UseCases;
using KatsumiApp.V1.Application.Exceptions.Post;
using Microsoft.EntityFrameworkCore;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.V1.Application.Features.Post.Shared.UseCases
{
    public class MakeSharedPost
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PostContext.SharedPostContext _sharedPostContext;
            private readonly PostContext.RegularPostContext _regularPostContext;

            public Handler(PostContext.SharedPostContext sharedPostContext, PostContext.RegularPostContext regularPostContext)
            {
                _sharedPostContext = sharedPostContext ?? throw new ArgumentNullException(nameof(sharedPostContext));
                _regularPostContext = regularPostContext ?? throw new ArgumentNullException(nameof(regularPostContext)); ;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var originalPost = await _regularPostContext.RegularPosts.FirstOrDefaultAsync(p => p.Id == command.OrigitalPostId, cancellationToken);

                if (originalPost == null)
                {
                    throw new OriginalPostNotFoundException(command.OrigitalPostId);
                }

                var sharing = command.MapToDomain();

                await _sharedPostContext.AddAsync(sharing, cancellationToken);
                await _sharedPostContext.SaveChangesAsync(cancellationToken);

                var result = sharing.MapFromDomain();

                return result;
            }
        }

        public class Command : IRequest<Result>
        {
            public string OrigitalPostId { get; set; }
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
                OriginalPostId = command.OrigitalPostId,    
            };
        }
    }
}
