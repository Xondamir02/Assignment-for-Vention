namespace BlogApi.Models.PostModels
{
    public class PostModel
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid BlogId { get; set; }

    }
}
