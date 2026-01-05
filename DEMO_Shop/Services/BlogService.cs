using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class BlogService
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public BlogService(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // ================= READ =================

        public List<Blog> GetActive()
        {
            return _context.Blogs
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.BlogId)
                .ToList();
        }

        public Blog? GetById(int id)
        {
            return _context.Blogs
                .Include(b => b.Detail)
                .FirstOrDefault(b => b.BlogId == id);
        }

        // ================= CREATE =================
        public async Task<Blog?> Create(BlogCreateUpdateDto dto, IFormFile imageFile)
        {
            if (imageFile == null)
                return null;

            // 1. Upload image
            var imageUrl = await _cloudinaryService.UploadImageAsync(imageFile);

            // 2. Create Blog
            var blog = new Blog
            {
                Title = dto.Title,
                ImageUrl = imageUrl,
                IsActive = dto.IsActive
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            // 3. Create BlogDetail
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
        public async Task<Blog?> Update(BlogCreateUpdateDto dto, IFormFile? imageFile)
        {
            if (!dto.BlogId.HasValue)
                return null;

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(x => x.BlogId == dto.BlogId.Value);

            if (blog == null)
                return null;

            // Upload new image if provided
            if (imageFile != null)
            {
                blog.ImageUrl = await _cloudinaryService.UploadImageAsync(imageFile);
            }

            blog.Title = dto.Title;
            blog.IsActive = dto.IsActive;

            var detail = await _context.BlogsDetail
                .FirstOrDefaultAsync(x => x.BlogId == blog.BlogId);

            if (detail == null)
                return null;

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
