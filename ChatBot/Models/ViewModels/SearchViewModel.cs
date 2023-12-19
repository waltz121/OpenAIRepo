using HtmlAgilityPack;
using OpenAiCore.OpenAiServices;
using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models.ViewModels
{
    public class SearchViewModel
    {
        OpenAiService openAiService;
        public SearchViewModel()
        {
            searchResults = new List<SearchResults>();
            openAiService = new OpenAiService();
        }

        private string searchText;
        private List<SearchResults> _searchResults;

        [Required(ErrorMessage = "Please enter a search text")]
        public string SearchText { get => searchText; set => searchText = value; }
        public List<SearchResults> searchResults { get => _searchResults; set => _searchResults = value; }

        public async Task GetSearchResults()
        {
           var pineconeResult = await openAiService.GetTop_Ranking_Pinecone(20, searchText);
            foreach (var result in pineconeResult.Matches)
            {
                var Title = await GetTitleFromUrl(result.Metadata.Url);
                searchResults.Add(new SearchResults(Title, result.Metadata.Text, result.Metadata.Url));
            }
        }
        public async Task<string> GetTitleFromUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var pageContents = await response.Content.ReadAsStringAsync();

                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                var titleNode = pageDocument.DocumentNode.SelectSingleNode("//head/title");

                return titleNode == null ? string.Empty : titleNode.InnerHtml;
            }
        }
        public void InsertTestData()
        {
            searchResults.Add(new SearchResults("Test Title 1", "Test Description 1", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 2", "Test Description 2", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 3", "Test Description 3", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 4", "Test Description 4", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 5", "Test Description 5", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 6", "Test Description 6", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 7", "Test Description 7", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 8", "Test Description 8", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 9", "Test Description 9", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 10", "Test Description 10", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 11", "Test Description 11", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 12", "Test Description 12", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 13", "Test Description 13", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 14", "Test Description 14", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 15", "Test Description 15", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 16", "Test Description 16", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 17", "Test Description 17", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 18", "Test Description 18", "https://www.google.com"));
            searchResults.Add(new SearchResults("Test Title 19", "Test Description 19", "https://www.google.com"));
        }
        
    }
}
