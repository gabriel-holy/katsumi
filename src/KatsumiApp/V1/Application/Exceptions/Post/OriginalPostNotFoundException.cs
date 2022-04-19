using System;

namespace KatsumiApp.V1.Application.Exceptions.Post
{
    public class OriginalPostNotFoundException : Exception
    {
        public OriginalPostNotFoundException(Guid OriginalPostId) : base($"Original post {OriginalPostId} not found to be shared or quoted.")
        {

        }
    }
}
