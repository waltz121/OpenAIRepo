namespace ChatBot.Models
{
    public class Prompt
    {
        private string openAiChatbotPrompt = "";

        public string GetOpenAiChatbotPrompt(string filename)
        {

            string text = File.ReadAllText(@"wwwroot\File\" + filename);
            openAiChatbotPrompt = text;
            return openAiChatbotPrompt;
        }
    }
}
