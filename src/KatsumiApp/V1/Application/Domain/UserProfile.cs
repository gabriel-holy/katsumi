using System;

namespace KatsumiApp.V1.Application.Domain
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime JoinedSocialMediaAt { get; set; }
    }
}
