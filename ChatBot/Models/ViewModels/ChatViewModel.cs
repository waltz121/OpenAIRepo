using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO;

namespace ChatBot.Models.ViewModels
{
    public class ChatViewModel
    {
        OpenAiAPIRepository OpenAiRepo;

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
                new MessagesDTO() { Role = "system", Content = "You are a helpful Customer Service Assistant from Mercola Named Fabio. You greet the user with your name and how you can assist them first thing."}
            };

            requestBody.Model = "gpt-3.5-turbo";
            requestBody.Messages = messagesDTO;

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response.Choices[0].Message;                       
            
        }

        public async Task<ChatCompletionResponseDTO> GetBotReply()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            requestBody.Messages = Messages.ToArray();

            requestBody.Model = "gpt-3.5-turbo";

            var response = await OpenAiRepo.ChatCompletion(requestBody);

            return response;
        }
        
    }
}
