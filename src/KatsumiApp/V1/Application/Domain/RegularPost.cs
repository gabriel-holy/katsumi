using System;
using System.Collections.Generic;
using KatsumiApp.V1.Application.Domain.Enum;
using KatsumiApp.V1.Application.Domain;

namespace KatsumiApp.V1.Application.Domain
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

        public record Content
        {
            public string Title { get; set; }
            public string Text { get; set; }
            public string Media { get; set; }
            public IEnumerable<string> Keywords { get; set; }
        }
    }
}
