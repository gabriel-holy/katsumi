using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using KatsumiApp.V1.Application.Models.Post;
using Microsoft.EntityFrameworkCore;
using KatsumiApp.V1.Application.Exceptions.Post;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.V1.Application.Features.Post.Quote.UseCases
{
    public class MakeQuotePost
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PostContext.QuotePostContext _quotePostContext;
            private readonly PostContext.RegularPostContext _regularPostContext;

            public Handler(
                PostContext.QuotePostContext quotePostContext,
                PostContext.RegularPostContext regularPostContext
                )
            {
                _quotePostContext = quotePostContext ?? throw new ArgumentNullException(nameof(quotePostContext));
                _regularPostContext = regularPostContext ?? throw new ArgumentNullException(nameof(regularPostContext));
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var originalPost = await _regularPostContext.RegularPosts.FirstOrDefaultAsync(p => p.Id == command.OrigitalPostId, cancellationToken);

                if (originalPost == null)
                {
                    throw new OriginalPostNotFoundException(command.OrigitalPostId);
                }

                var quote = command.MapToDomain();

                await _quotePostContext.AddAsync(quote, cancellationToken);
                await _quotePostContext.SaveChangesAsync(cancellationToken);

                var result = quote.MapFromDomain();

                return result;
            }
        }

        public class Command : IRequest<Result>
        {
            public string Username { get; set; }
            public string Comment { get; set; }
            public string OrigitalPostId { get; set; }

        }

        public class Result
        {
            public string Id { get; set; }
        }
    }

    public static class QuotePostMapper
    {
        public static MakeQuotePost.Result MapFromDomain(this QuotePost quotePost)
        {
            return new MakeQuotePost.Result()
            {
                Id = quotePost.Id,
            };
        }

        public static QuotePost MapToDomain(this MakeQuotePost.Command command)
        {
            return new QuotePost()
            {
                Username = command.Username,
                CreatedAtUtc = DateTime.UtcNow,
                Comment = command.Comment,
                OriginalPostId = command.OrigitalPostId,
            };
        }
    }
}
