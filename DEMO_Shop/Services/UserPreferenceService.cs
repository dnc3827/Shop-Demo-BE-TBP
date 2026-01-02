using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Models;

namespace DEMO_Shop.Services
{
    public class UserPreferenceService
    {
        private readonly AppDbContext _db;
        private readonly GoogleSheetService _sheet;

        public UserPreferenceService(AppDbContext db, GoogleSheetService sheet)
        {
            _db = db;
            _sheet = sheet;
        }

        public void Create(UserPreferenceDto dto)
        {
            var entity = new UserPreference
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                Description = dto.Description,
                CreatedAt = DateTime.Now
            };

            _db.UserPreferences.Add(entity);
            _db.SaveChanges();

            // Đẩy sang Google Sheet
            _sheet.AddPreference(entity);
        }
    }
}
