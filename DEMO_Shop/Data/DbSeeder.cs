using DEMO_Shop.Helpers;
using DEMO_Shop.Models;

namespace DEMO_Shop.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmin(AppDbContext db)
        {
            if (db.Users.Any(x => x.Username == "admin"))
                return; // 

            db.Users.Add(new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = PasswordHelper.Hash("123456"),
                Role = "Admin"
            });

            db.SaveChanges();
        }
    }
}
