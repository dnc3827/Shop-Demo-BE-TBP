using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Helpers;
using DEMO_Shop.Models;

namespace DEMO_Shop.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public User Register(RegisterDto dto)
        {
            if (_db.Users.Any(x => x.Username == dto.Username))
                throw new Exception("USERNAME_EXISTS");

            if (_db.Users.Any(x => x.Email == dto.Email))
                throw new Exception("EMAIL_EXISTS");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = PasswordHelper.Hash(dto.Password),
                Role = "Customer"
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }

        public User Login(LoginDto dto)
        {
            var user = _db.Users.FirstOrDefault(x =>
                x.Username == dto.Username || x.Email == dto.Username);

            if (user == null || !PasswordHelper.Verify(dto.Password, user.PasswordHash))
                throw new Exception("INVALID_CREDENTIAL");

            return user;
        }
    }
}
