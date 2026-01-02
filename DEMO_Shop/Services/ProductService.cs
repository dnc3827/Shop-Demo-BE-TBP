using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Migrations;
using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public List<Product> GetPaged(int page, int size)
        {
            if (page <= 0) page = 1;
            if (size <= 0) size = 10;

            return _db.Products
                .AsNoTracking()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }

        public ProductDetailDto? GetById(int id)
        {
            return _db.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Category = new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name,
                        Slug = p.Category.Slug
                    }
                })
                .FirstOrDefault();
        }

        public List<Product> Search(string keyword)
            => _db.Products
                .Where(x => x.Name.Contains(keyword))
                .ToList();

        public void Create(DTOs.CreateProductDto dto)
        {
            var categoryExists = _db.Categories.Any(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                throw new Exception("Category không tồn tại");

            _db.Products.Add(new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,   // ✅
                CategoryId = dto.CategoryId
            });

            _db.SaveChanges();
        }

        public void Update(int id, DTOs.UpdateProductDto dto)
        {
            var product = _db.Products.Find(id);

            if (product == null)
                throw new Exception("Sản phẩm không tồn tại");

            if (!_db.Categories.Any(c => c.Id == dto.CategoryId))
                throw new Exception("Danh mục không tồn tại");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
                throw new Exception("Sản phẩm không tồn tại");

            _db.Products.Remove(product);
            _db.SaveChanges();
        }
    }
}
