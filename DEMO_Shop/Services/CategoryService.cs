using DEMO_Shop.Data;
using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET ALL =================
        public List<Category> GetAll()
        {
            return _db.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.Id)
                .ToList();
        }

        // ================= GET BY ID =================
        public CategoryDetailDto? GetById(int id)
        {
            return _db.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDetailDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    IsActive = c.IsActive
                })
                .FirstOrDefault();
        }

        // ================= CREATE =================
        public void Create(CreateCategoryDto dto)
        {
            if (_db.Categories.Any(c => c.Slug == dto.Slug))
                throw new Exception("Slug đã tồn tại");

            var category = new Category
            {
                Name = dto.Name,
                Slug = dto.Slug,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        // ================= UPDATE =================
        public void Update(int id, UpdateCategoryDto dto)
        {
            var category = _db.Categories.Find(id);

            if (category == null)
                throw new Exception("Danh mục không tồn tại");

            if (_db.Categories.Any(c => c.Slug == dto.Slug && c.Id != id))
                throw new Exception("Slug đã tồn tại");

            category.Name = dto.Name;
            category.Slug = dto.Slug;
            category.IsActive = dto.IsActive;

            _db.SaveChanges();
        }

        // ================= DELETE =================
        public void Delete(int id)
        {
            var category = _db.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                throw new Exception("Danh mục không tồn tại");

            if (category.Products.Any())
                throw new Exception("Không thể xóa danh mục đang chứa sản phẩm");

            _db.Categories.Remove(category);
            _db.SaveChanges();
        }
    }

    // ================= DTOs =================

    public class CreateCategoryDto
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public bool IsActive { get; set; }
    }
    public class CategoryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
