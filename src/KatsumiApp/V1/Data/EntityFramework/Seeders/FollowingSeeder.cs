using ForEvolve.EntityFrameworkCore.Seeders;
using KatsumiApp.V1.Application.Models;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.V1.Data.EntityFramework.Seeders
{
    public class FollowingSeeder : ISeeder<FollowingContext>
    {
        public void Seed(FollowingContext db)
        {
            #region Gabriel followers

            db.Followings.Add(new Following
            {
                FollowedUsername = "gabriel",
                FollowerUsername = "adam",
                FollowingIsActive = false,
            });

            db.Followings.Add(new Following
            {
                FollowedUsername = "gabriel",
                FollowerUsername = "steve",
                FollowingIsActive = true,
            });

            db.Followings.Add(new Following
            {
                FollowedUsername = "gabriel",
                FollowerUsername = "lucas",
                FollowingIsActive = true,
            });
            #endregion

            #region Steve followers

            db.Followings.Add(new Following
            {
                FollowedUsername = "steve",
                FollowerUsername = "gabriel",
                FollowingIsActive = true,
            });

            db.Followings.Add(new Following
            {
                FollowedUsername = "steve",
                FollowerUsername = "lucas",
                FollowingIsActive = true,
            });
            #endregion

            #region Lucas followers

            db.Followings.Add(new Following
            {
                FollowedUsername = "lucas",
                FollowerUsername = "gabriel",
                FollowingIsActive = true,
            });
            #endregion

            // Adam has no followers yet

            db.SaveChanges();
        }
    }
}
