using ForEvolve.EntityFrameworkCore.Seeders;
using KatsumiApp.V1.Application.Models;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp.V1.Data.EntityFramework.Seeders
{
    public class UserProfileSeeder : ISeeder<UserProfileContext>
    {
        public void Seed(UserProfileContext db)
        {
            db.UsersProfiles.Add(new UserProfile
            {
                Username = "adam",
                JoinedSocialMediaAt = new System.DateTime(2015, 06, 20),

            });

            db.UsersProfiles.Add(new UserProfile
            {
                Username = "gabriel",
                JoinedSocialMediaAt = new System.DateTime(2016, 11, 01)
            });

            db.UsersProfiles.Add(new UserProfile
            {
                Username = "lucas",
                JoinedSocialMediaAt = new System.DateTime(2020, 03, 10)
            });

            db.UsersProfiles.Add(new UserProfile
            {
                Username = "steve",
                JoinedSocialMediaAt = new System.DateTime(2021, 03, 27)
            });

            db.SaveChanges();
        }
    }
}
