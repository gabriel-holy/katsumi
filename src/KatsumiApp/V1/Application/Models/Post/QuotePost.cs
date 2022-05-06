using KatsumiApp.V1.Application.Models.Post.Enum;
using System;

namespace KatsumiApp.V1.Application.Models.Post
{
    public class QuotePost
    {
        private DateTime _createdAt;
        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAtUtc { get => _createdAt.ToUniversalTime(); set => _createdAt = value; }
        public string Type { get => PostType.Quote.ToString(); }
        public string Comment { get; set; }
        public string OriginalPostId { get; set; }
        public RegularPost OriginalPost { get; set; }
    }
}
