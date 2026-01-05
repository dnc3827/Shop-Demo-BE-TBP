using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class BlogService
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService; // Thêm dòng này

        public BlogService(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // Admin list
        // ================= READ =================

        public List<Blog> GetAll()
        {
            return _context.Blogs
                .OrderByDescending(b => b.BlogId)
                .ToList();
        }

        public List<Blog> GetActive()
        {
            return _context.Blogs
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.BlogId)
                .ToList();
        }

        public Blog GetById(int id)
        {
            return _context.Blogs
                .Include(b => b.Detail)
                .FirstOrDefault(b => b.BlogId == id);
        }

        // ================= CREATE =================
        public async Task<Blog> Create(BlogCreateUpdateDto dto, IFormFile imageFile)
        {
            if (imageFile == null)
                throw new Exception("Vui lòng upload hình ảnh");

            // 1. Upload Cloudinary
            var imageUrl = await _cloudinaryService.UploadImageAsync(imageFile);

            // 2. Tạo Blog
            var blog = new Blog
            {
                Title = dto.Title,
                ImageUrl = imageUrl,
                IsActive = dto.IsActive
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            // 3. Tạo BlogDetail
            var detail = new BlogDetail
            {
                BlogId = blog.BlogId,
                Content = dto.Content
            };

            _context.BlogsDetail.Add(detail);
            await _context.SaveChangesAsync();

            return blog;
        }


        // ================= UPDATE =================
        public async Task<Blog> Update(BlogCreateUpdateDto dto, IFormFile? imageFile)
        {
            if (!dto.BlogId.HasValue)
                throw new Exception("BlogId không hợp lệ");

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(x => x.BlogId == dto.BlogId.Value)
                ?? throw new Exception("Blog không tồn tại");

            // 1. Upload ảnh mới nếu có
            if (imageFile != null)
            {
                blog.ImageUrl = await _cloudinaryService.UploadImageAsync(imageFile);
            }
            // nếu không upload → giữ ảnh cũ

            // 2. Update thông tin
            blog.Title = dto.Title;
            blog.IsActive = dto.IsActive;

            // 3. Update BlogDetail
            var detail = await _context.BlogsDetail
                .FirstOrDefaultAsync(x => x.BlogId == blog.BlogId)
                ?? throw new Exception("BlogDetail không tồn tại");

            detail.Content = dto.Content;

            await _context.SaveChangesAsync();

            return blog;
        }


        // ================= DELETE =================
        public bool Delete(int id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null) return false;

            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return true;
        }
    }
}
