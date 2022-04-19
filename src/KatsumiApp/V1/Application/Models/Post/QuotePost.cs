using KatsumiApp.V1.Application.Models.Post.Base;
using KatsumiApp.V1.Application.Models.Post.Enum;
using System;

namespace KatsumiApp.V1.Application.Models.Post
{
    public class QuotePost : Base.Post
    {
        public override string Type { get => PostType.Quote.ToString(); }
        public string Comment { get; set; }
        public Guid OriginalPostId { get; set; }
        public RegularPost OriginalPost { get; set; }
    }
}
