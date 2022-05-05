using Raven.Client.Documents;
using System;

namespace KatsumiApp.V1.Data.Raven.Contexts
{
    public static class FollowingContext
    {
        private static readonly Lazy<IDocumentStore> _lazyFollowing = new(() =>
        {
            IDocumentStore following = new DocumentStore()
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "katsumiDB",
            };

            following.Initialize();

            return following;
        });


        public static IDocumentStore Following => _lazyFollowing.Value;
    }
}
