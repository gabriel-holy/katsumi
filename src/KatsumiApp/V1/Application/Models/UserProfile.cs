using System;

namespace KatsumiApp.V1.Application.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            if (Id == Guid.Empty) { Id = Guid.NewGuid(); }
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime JoinedSocialMediaAt { get; set; }
    }
}
