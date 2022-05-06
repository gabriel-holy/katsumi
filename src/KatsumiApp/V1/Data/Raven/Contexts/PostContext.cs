using System;
using Raven.Client.Documents;

namespace KatsumiApp.V1.Data.Raven.Contexts
{
    public class PostContext
    {
        public static class QuotePostContext
        {
            private static readonly Lazy<IDocumentStore> _lazyQuotePost = new(() =>
            {
                IDocumentStore quotePost = new DocumentStore()
                {
                    Urls = new[] { "http://127.0.0.1:8080/" },
                    Database = "katsumiDB",
                };

                quotePost.Initialize();

                return quotePost;
            });

            public static IDocumentStore DocumentStore => _lazyQuotePost.Value;
        }

        public static class RegularPostContext
        {
            private static readonly Lazy<IDocumentStore> _lazyRegularPost = new(() =>
            {
                IDocumentStore regularPost = new DocumentStore()
                {
                    Urls = new[] { "http://127.0.0.1:8080/" },
                    Database = "katsumiDB",
                };

                regularPost.Initialize();

                return regularPost;
            });

            public static IDocumentStore DocumentStore => _lazyRegularPost.Value;
        }

        public static class SharedPostContext
        {
            private static readonly Lazy<IDocumentStore> _lazySharedPost = new(() =>
            {
                IDocumentStore sharedPost = new DocumentStore()
                {
                    Urls = new[] { "http://127.0.0.1:8080/" },
                    Database = "katsumiDB",
                };

                sharedPost.Initialize();

                return sharedPost;
            });

            public static IDocumentStore DocumentStore => _lazySharedPost.Value;
        }
    }
}
