using OpenAiCore;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiServices;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class OpenAiServicesSpecs
    {
        OpenAiService OpenAiService;
        private const string prompt = "You are a helpful Customer Service Assistant from Mercola Named Fabio." +
           "As a Customer Service Assistant you have a cheerful and joyful personality it shows on your reply." +
           "Answer as clear, concise and succint as possible.";
        public OpenAiServicesSpecs()
        {
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "1000f1c9-9a38-471a-bdc5-483957668b0d");
            OpenAiService = new OpenAiService();
        }

        [TestMethod]
        public void GetAiResponseWith_FileContext()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            List<MessagesDTO> messages = new List<MessagesDTO>
            {
                new MessagesDTO { Role ="system", Content = prompt },
                new MessagesDTO() { Role = "assistant", Content = "Hello! How can I assist you today?" },
                new MessagesDTO() { Role = "user", Content = "Can you tell me about Mercola?" },
                new MessagesDTO() { Role = "assistant", Content = "Certainly! Mercola is a health and wellness company founded by Dr. Joseph Mercola. We are dedicated to providing high-quality products and information to help people lead healthier lives. Our products range from supplements to personal care items, all made with natural ingredients. We also offer a wealth of health-related articles and resources on our website. If you have any specific questions or need assistance with our products or services, feel free to ask!" }
            };
            requestBody.Messages = messages;
            requestBody.MaxTokens = 500;
            requestBody.Model = "gpt-3.5-turbo";


            string userMessage = "What does having low vitamin D Means?";

            Task.Run(async () =>
            {
                var response = await OpenAiService.GetChatCompletion_withSearch(requestBody, userMessage);
            }).GetAwaiter().GetResult();
        }

    }
}
