using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiServices;

namespace ChatBot.Models.ViewModels
{
    public class ChatViewModel
    {
        OpenAiAPIRepository OpenAiRepo;
        OpenAiService openAiService;
        private const string prompt = "You are a helpful Customer Service Assistant from Mercola Named Fabio." + 
            "As a Customer Service Assistant you have a cheerful and joyful personality it shows on your reply." + 
            "Answer as clear, concise and succint as possible." +
            "Format your answers as Html tags.";

        public ChatViewModel() {
            OpenAiRepo = new OpenAiAPIRepository();
            openAiService = new OpenAiService();
        }
        public List<MessagesDTO> Messages { get; set; }
        public string TxtMessage { get; set; }

        public async Task<MessagesDTO> SetInitialMessage()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            List<MessagesDTO> messagesDTO = new List<MessagesDTO>
            {
                new MessagesDTO() { Role = "system", Content = prompt }
            };

            requestBody.Model = "gpt-3.5-turbo";
            requestBody.Messages = messagesDTO;
            requestBody.MaxTokens = 200;
            requestBody.Temperature = 0.2;

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response.Choices[0].Message;                       
            
        }

        public async Task<ChatCompletionResponseDTO> GetBotReply()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            Messages.Insert(0, new MessagesDTO() { Role = "system", Content = prompt });
            requestBody.Messages = Messages;
            requestBody.MaxTokens = 1000;
            requestBody.Model = "gpt-3.5-turbo";

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response;
        }

        public async Task<ChatCompletionResponseDTO> GetBotReply_WithContext()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            ChatCompletionResponseDTO response = new ChatCompletionResponseDTO();
            Messages.Insert(0, new MessagesDTO() { Role = "system", Content = prompt });
            requestBody.Messages = Messages;
            requestBody.MaxTokens = 500;
            requestBody.Model = "gpt-3.5-turbo";
            requestBody.Temperature = 0.2;

            var LastMessage = Messages.Last();

            if (LastMessage.Role == "user")
            {
                response = await openAiService.GetChatCompletion_WithSearch_PineCone(requestBody, LastMessage.Content);
            }

            return response;
        }
        
    }
}
