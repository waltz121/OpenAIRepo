namespace ChatBot.Models
{
    public class SearchResults
    {
        private string title;
        private string description;
        private string url;

        public SearchResults()
        {
            // Initialize all private string variables
            title = "";
            description = "";
            url = "";
        }
        public SearchResults(string title, string description, string url)
        {
            this.title = title;
            this.description = description;
            this.url = url;
        }

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string Url { get => url; set => url = value; }
    }
}