using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class BlogService
    {
        private readonly AppDbContext _context;
        public BlogService(AppDbContext context)
        {
            _context = context;
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
        public Blog Create(BlogCreateUpdateDto dto)
        {
            var blog = new Blog
            {
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive
            };

            _context.Blogs.Add(blog);
            _context.SaveChanges();

            var detail = new BlogDetail
            {
                BlogId = blog.BlogId,
                Content = dto.Content
            };

            _context.BlogsDetail.Add(detail);
            _context.SaveChanges();

            return blog;
        }

        // ================= UPDATE =================
        public bool Update(BlogCreateUpdateDto dto)
        {
            var blog = _context.Blogs
                .Include(b => b.Detail)
                .FirstOrDefault(b => b.BlogId == dto.BlogId);

            if (blog == null) return false;

            blog.Title = dto.Title;
            blog.ImageUrl = dto.ImageUrl;
            blog.IsActive = dto.IsActive;
            blog.Detail.Content = dto.Content;

            _context.SaveChanges();
            return true;
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
