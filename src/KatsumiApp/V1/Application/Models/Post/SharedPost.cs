using KatsumiApp.V1.Application.Models.Post.Enum;
using System;

namespace KatsumiApp.V1.Application.Models.Post
{
    public class SharedPost : Base.Post
    {
        public override string Type { get => PostType.Shared.ToString(); }
        public RegularPost OriginalPost { get; set; }
        public Guid OriginalPostId { get; set; }
    }
}