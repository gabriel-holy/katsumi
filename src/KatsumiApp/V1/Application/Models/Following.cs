using System;

namespace KatsumiApp.V1.Application.Models
{
    public class Following
    {
        public string Id { get; private set; }
        public string FollowerUsername { get; set; }
        public string FollowedUsername { get; set; }
        public bool FollowingIsActive { get; set; }
    }
}
