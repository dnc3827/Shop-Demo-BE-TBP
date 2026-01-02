namespace DEMO_Shop.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = false; // chỉ ẩn đi thôi

        public BlogDetail Detail { get; set; }


    }
}
