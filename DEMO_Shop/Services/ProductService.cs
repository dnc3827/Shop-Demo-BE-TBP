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
        private readonly CloudinaryService _cloudinaryService; // Thêm dòng này

        public ProductService(AppDbContext db, CloudinaryService cloudinaryService) // Inject vào đây
        {
            _db = db;   
            _cloudinaryService = cloudinaryService;
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

        // Đổi void thành async Task
        public async Task Create(DTOs.CreateProductDto dto, IFormFile imageFile)
        {
            var categoryExists = _db.Categories.Any(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                throw new Exception("Category không tồn tại");

            // 1. Gọi service upload ảnh lên Cloudinary
            string imageUrl = dto.ImageUrl; // Mặc định dùng link cũ nếu không có file
            if (imageFile != null)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(imageFile);
            }

            // 2. Lưu vào DB với Link Cloudinary mới
            _db.Products.Add(new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = imageUrl, // Link https://res.cloudinary.com/...
                Description = dto.Description,
                CategoryId = dto.CategoryId
            });

            await _db.SaveChangesAsync();
        }

        public async Task Update(int id, DTOs.UpdateProductDto dto, IFormFile imageFile)
        {
            var product = _db.Products.Find(id);
            if (product == null) throw new Exception("Sản phẩm không tồn tại");

            if (!_db.Categories.Any(c => c.Id == dto.CategoryId))
                throw new Exception("Danh mục không tồn tại");

            // Nếu có file ảnh mới thì upload, không thì dùng lại product.ImageUrl cũ
            if (imageFile != null)
            {
                product.ImageUrl = await _cloudinaryService.UploadImageAsync(imageFile);
            }

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;

            await _db.SaveChangesAsync();
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
