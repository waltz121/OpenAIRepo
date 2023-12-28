
using OpenAiCore.UploadPineConeServices;

namespace ChatBot.Models.ViewModels
{
    public class UploadViewModel
    {
        UploadServices _uploadServices;
        public UploadViewModel()
        {
            _uploadServices = new UploadServices();
        }

        public UploadViewModel(string url, string resultMessage)
        {
            this.url = url;
            this.resultMessage = resultMessage;
        }

        private string url;
        private string resultMessage = "";

        public string Url { get => url; set => url = value; }
        public string ResultMessage { get => resultMessage; set => resultMessage = value; }

        public async Task SaveUrl()
        {
            // Code for getting the Html from a url link.
            resultMessage = await _uploadServices.SaveHtmlInput_To_PineCone(url);
        }
    }
}
