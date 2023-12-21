
namespace ChatBot.Models.ViewModels
{
    public class UploadViewModel
    {
        public UploadViewModel()
        {

        }

        public UploadViewModel(string url, string resultMessage)
        {
            this.url = url;
            this.resultMessage = resultMessage;
        }

        private string url;
        private string resultMessage;

        public string Url { get => url; set => url = value; }
        public string ResultMessage { get => resultMessage; set => resultMessage = value; }

        public void SaveUrl()
        {
            // Code for getting the Html from a url link.
                        

        }
    }
}
