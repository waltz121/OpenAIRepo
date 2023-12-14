using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiServices;

namespace ChatBot.Models.ViewModels
{
    public class ChatViewModel
    {
        OpenAiAPIRepository OpenAiRepo;
        OpenAiService openAiService;
        Prompt prompt;
        private List<MessagesDTO> InitialMessages;
       
        public ChatViewModel() {
            OpenAiRepo = new OpenAiAPIRepository();
            openAiService = new OpenAiService();
            prompt = new Prompt();
            prompt.GetOpenAiChatbotPrompt();
            InitialMessages = new List<MessagesDTO>()
            {
                new MessagesDTO() { Role = "system", Content = prompt.GetOpenAiChatbotPrompt() },
                new MessagesDTO() { Role = "user", Content = "Hello!" }
            };
            InitialMessages.Reverse();
        }
        public List<MessagesDTO> Messages { get; set; }
        public string TxtMessage { get; set; }

        public async Task<MessagesDTO> SetInitialMessage()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            List<MessagesDTO> messagesDTO = InitialMessages;

            requestBody.Model = "gpt-3.5-turbo-1106";
            requestBody.Messages = messagesDTO;
            requestBody.MaxTokens = 500;
            requestBody.Temperature = 0.2;

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response.Choices[0].Message;                       
            
        }

        public async Task<ChatCompletionResponseDTO> GetBotReply()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            requestBody.Messages = InitialMessages;
            requestBody.MaxTokens = 500;
            requestBody.Model = "gpt-3.5-turbo";

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response;
        }

        public async Task<ChatCompletionResponseDTO> GetBotReply_WithContext()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            ChatCompletionResponseDTO response = new ChatCompletionResponseDTO();

            foreach (var message in InitialMessages)
            {
                Messages.Insert(0, message);
            }

            requestBody.Messages = Messages;
            requestBody.MaxTokens = 2000;
            requestBody.Model = "gpt-3.5-turbo-1106";
            requestBody.Temperature = 0.1;

            var LastMessage = Messages.Last();
            requestBody.Messages.Add(LastMessage);

            if (LastMessage.Role == "user")
            {
                response = await openAiService.GetChatCompletion_WithSearch_PineCone(requestBody, LastMessage.Content);
            }

            return response;
        }
        
    }
}
