using System;
using KatsumiApp.V1.Application.Domain.Enum;
using KatsumiApp.V1.Application.Domain;

namespace KatsumiApp.V1.Application.Domain
{
    public class SharedPost
    {
        private DateTime _createdAt;

        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAtUtc { get => _createdAt.ToUniversalTime(); set => _createdAt = value; }
        public string Type { get => PostType.Shared.ToString(); }
        public RegularPost OriginalPost { get; set; }
        public string OriginalPostId { get; set; }
    }
}
