namespace Ass3.Models
{
    public class Post
    {

        public int PostID { get; set; }
        public int AuthorID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool PublishStatus { get; set; }
        public int CategoryID { get; set; }

        public virtual AppUser Author { get; set; }
        public virtual PostCategory Category { get; set; }
    }
}
