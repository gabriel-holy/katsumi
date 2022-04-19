using System;

namespace KatsumiApp.V1.Application.Models.Post.Base
{
    public abstract class Post
    {
        private DateTime _createdAt;

        public Post()
        {
            if (Id == Guid.Empty) { Id = Guid.NewGuid(); }
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAtUtc { get => _createdAt.ToUniversalTime(); set => _createdAt = value; }
        public abstract string Type { get; }
    }
}
