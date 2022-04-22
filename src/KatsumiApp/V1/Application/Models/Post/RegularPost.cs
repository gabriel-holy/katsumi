using KatsumiApp.V1.Application.Models.Post.Enum;
using System;
using System.Collections.Generic;

namespace KatsumiApp.V1.Application.Models.Post
{
    public class RegularPost
    {
        private DateTime _createdAt;

        public RegularPost()
        {
            if (Id == Guid.Empty) { Id = Guid.NewGuid(); }

            PostContent ??= new Content();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAtUtc { get => _createdAt.ToUniversalTime(); set => _createdAt = value; }
        public string Type { get => PostType.Regular.ToString(); }
        public Content PostContent { get; set; }

        public class Content
        {
            public Content()
            {
                if (Id == Guid.Empty) { Id = Guid.NewGuid(); }
            }
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string Media { get; set; }
            public IEnumerable<Keyword> Keywords { get; set; }

            public class Keyword
            {
                public Keyword()
                {
                    if (Id == Guid.Empty) { Id = Guid.NewGuid(); }
                }

                public Keyword(string keyvalue)
                {
                    if (Id == Guid.Empty) { Id = Guid.NewGuid(); }

                    Keyvalue = keyvalue;
                }

                public Guid Id { get; set; }
                public string Keyvalue { get; set; }
            }
        }
    }


}