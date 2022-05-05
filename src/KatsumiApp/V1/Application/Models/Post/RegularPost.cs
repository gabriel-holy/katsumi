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
            PostContent ??= new Content();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAtUtc { get => _createdAt.ToUniversalTime(); set => _createdAt = value; }
        public string Type { get => PostType.Regular.ToString(); }
        public Content PostContent { get; set; }

        public class Content
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string Media { get; set; }
            public IEnumerable<Keyword> Keywords { get; set; }

            public class Keyword
            {
                public Keyword(string keyvalue)
                {
                    Keyvalue = keyvalue;
                }

                public string Id { get; set; }
                public string Keyvalue { get; set; }
            }
        }
    }


}