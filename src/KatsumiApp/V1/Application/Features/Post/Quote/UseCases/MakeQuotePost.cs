﻿using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using KatsumiApp.V1.Application.Models.Post;
using KatsumiApp.V1.Application.Exceptions.Post;
using Raven.Client.Documents;
using KatsumiApp.V1.Data.Raven.Contexts;

namespace KatsumiApp.V1.Application.Features.Post.Quote.UseCases
{
    public class MakeQuotePost
    {
        public class Handler : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                using var quotePostDatabaseSession = PostContext.QuotePostContext.DocumentStore.OpenAsyncSession();

                var originalPost = await quotePostDatabaseSession
                                         .Query<QuotePost>()
                                         .FirstOrDefaultAsync(p => p.Id == command.OriginalPostId, cancellationToken);

                if (originalPost == null)
                {
                    throw new OriginalPostNotFoundException(command.OriginalPostId);
                }

                var quote = command.MapToDomain();

                await quotePostDatabaseSession.StoreAsync(quote, cancellationToken);

                await quotePostDatabaseSession.SaveChangesAsync(cancellationToken);

                var result = quote.MapFromDomain();

                return result;
            }
        }

        public class Command : IRequest<Result>
        {
            public string Username { get; set; }
            public string Comment { get; set; }
            public string OriginalPostId { get; set; }

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
                OriginalPostId = command.OriginalPostId,
            };
        }
    }
}
