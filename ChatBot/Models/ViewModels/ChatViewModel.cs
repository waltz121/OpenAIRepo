using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO;

namespace ChatBot.Models.ViewModels
{
    public class ChatViewModel
    {
        OpenAiAPIRepository OpenAiRepo;
        private const string prompt = "You are a helpful Customer Service Assistant from Mercola Named Fabio." + 
            "As a Customer Service Assistant you have a cheerful and joyful personality it shows on your reply." + 
            "Answer as clear, concise and succint as possible.";

        public ChatViewModel() {
            OpenAiRepo = new OpenAiAPIRepository();
        }
        public List<MessagesDTO> Messages { get; set; }
        public string TxtMessage { get; set; }

        public async Task<MessagesDTO> SetInitialMessage()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            MessagesDTO[] messagesDTO = new MessagesDTO[]
            {
                new MessagesDTO() { Role = "system", Content = prompt }
            };

            requestBody.Model = "gpt-3.5-turbo";
            requestBody.Messages = messagesDTO;

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response.Choices[0].Message;                       
            
        }

        public async Task<ChatCompletionResponseDTO> GetBotReply()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            Messages.Insert(0, new MessagesDTO() { Role = "system", Content = prompt });
            requestBody.Messages = Messages.ToArray();
            requestBody.MaxTokens = 500;
            requestBody.Model = "gpt-3.5-turbo";

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response;
        }
        
    }
}
