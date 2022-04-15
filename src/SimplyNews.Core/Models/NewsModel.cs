namespace SimplyNews.Core.Models
{
    public class NewsRootobject
    {
        public string category { get; set; }
        public NewsItem[] data { get; set; }
        public bool success { get; set; }
    }

    public class NewsItem
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string ImageUrl { get; set; }
        public string ReadMoreUrl { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
