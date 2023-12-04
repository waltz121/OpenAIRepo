namespace ChatBot.Models.ViewModels
{
    public class ChatViewModel
    {
        private string content {  get; set; }

        public ChatViewModel() {
            Content = "<div class=\"messages\"><div class=\"messages-content\"></div></div>";
        }
        public string Content { get; set; }
        public string TxtMessage { get; set; }

    }
}
