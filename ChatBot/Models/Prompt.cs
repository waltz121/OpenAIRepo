namespace ChatBot.Models
{
    public class Prompt
    {
        private string openAiChatbotPrompt = "";

        public string GetOpenAiChatbotPrompt()
        {
            string text = File.ReadAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\ChatBot\wwwroot\File\OpenAiChatbotPrompt.txt");
            openAiChatbotPrompt = text;
            return openAiChatbotPrompt;
        }
    }
}
