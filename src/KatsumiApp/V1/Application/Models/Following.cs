using System;

namespace KatsumiApp.V1.Application.Models
{
    public class Following
    {
        public Following()
        {
            if (Id == Guid.Empty) { Id = Guid.NewGuid(); }
        }
        public Guid Id { get; private set; }
        public string FollowerUsername { get; set; }
        public string FollowedUsername { get; set; }
        public bool FollowingIsActive { get; set; }
    }
}
