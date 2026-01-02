namespace DEMO_Shop.Models
{
    public class BlogDetail
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string Content { get; set; }

     
        public Blog Blog { get; set; }
    }
}
