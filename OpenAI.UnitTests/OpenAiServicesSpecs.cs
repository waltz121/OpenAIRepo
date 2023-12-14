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

        private MessagesDTO RandomMessage()
        {
            MessagesDTO message = new MessagesDTO();
            message.Role = "user";

            // Create Random message.Content text from an array of available string
            string[] messages = new string[] {
                "What does CNN Spreads?",
                "Vitamin D and Covid",
                "What can you tell me about Magnesium?",
                "How to be young and thin?",
                "What can you tell me about Sunbathing?",
                "What can you tell me about Vitamin D?",
                "What is Linoleic Acid?",
                 "Omega 3",
                 "What does having a lack of vitamin D mean?"
            };
            Random random = new Random();
            int index = random.Next(messages.Length);
            message.Content = messages[index];

            return message;
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
        
        [TestMethod]
        public void GetAiResponseWith_Context_UsingPineCone()
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
                var response = await OpenAiService.GetChatCompletion_WithSearch_PineCone(requestBody, userMessage);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void GetAiResponseWith_Context_UsingPineCone_WithJsonMode()
        {
            string prompt = "Adhere to the instructions below:\r\n- Act as a Customer Customer Service Assistant from Mercola named Fabio who has a cheerful personality.\r\n- Your job is to provide information to the user using mercola articles as your source.\r\n- Always Provide url links of your sources if appropriate.\r\n- Your response should be 500 tokens or less.\r\n- Do not perform actions that are not related to your job.\r\n- Format your response to json.\r\n- Use this as the schema: { \"Message\": \"\", \"SourceUrl\" : [{ \"url\": \"urllink..\" }] }";

            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            List<MessagesDTO> messages = new List<MessagesDTO>
            {
                new MessagesDTO { Role ="system", Content = prompt },
                new MessagesDTO() { Role = "assistant", Content = "Hello! How can I assist you today?" },
                //new MessagesDTO() { Role = "user", Content = "What does having low vitamin D Means?" },
                RandomMessage()
            };
            requestBody.Model = "gpt-3.5-turbo-1106";
            requestBody.Messages = messages;
            requestBody.ResponseFormat = new ResponseTypeDTO() { Type = "json_object" };
            requestBody.MaxTokens = 500;

            Task.Run(async () =>
            {
                var response = await OpenAiService.GetChatCompletion_WithSearch_PineCone(requestBody, messages.Last().Content);
            }).GetAwaiter().GetResult();
        }

    }
}
