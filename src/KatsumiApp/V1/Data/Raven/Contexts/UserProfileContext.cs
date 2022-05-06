using Raven.Client.Documents;
using System;

namespace KatsumiApp.V1.Data.Raven.Contexts
{
    public static class UserProfileContext
    {
        private static readonly Lazy<IDocumentStore> _lazyUserProfile = new(() =>
        {
            IDocumentStore userProfile = new DocumentStore()
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "katsumiDB",
            };

            userProfile.Initialize();

            return userProfile;
        });

        public static IDocumentStore DocumentStore => _lazyUserProfile.Value;
    }
}
