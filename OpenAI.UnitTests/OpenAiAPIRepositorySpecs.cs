using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class OpenAiAPIRepositorySpecs
    {
        OpenAiAPIRepository OpenAiRepo;
        public OpenAiAPIRepositorySpecs() {
            Config.Init("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");
            OpenAiRepo = new OpenAiAPIRepository();
        }

        [TestMethod]
        public void ChatCompletion()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            MessagesDTO[] messagesDTO = new MessagesDTO[]
            {
                new MessagesDTO() { Role = "user", Content = "Who won the world series in 2020?" },
                new MessagesDTO() { Role = "assistant", Content = "The Los Angeles Dodgers won the World Series in 2020." },
                new MessagesDTO() { Role = "user", Content = "Where was it played?" }
            };

            requestBody.Model = "gpt-3.5-turbo";
            requestBody.Messages = messagesDTO;            

            Task.Run(async () =>
            {
              var response = await OpenAiRepo.ChatCompletion(requestBody);
            }).GetAwaiter().GetResult();
            
        }

        [TestMethod]
        public void Mercola_AI_Assistant()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            MessagesDTO[] messagesDTOs = new MessagesDTO[]
            {
                new MessagesDTO() { Role = "system", Content = "" }
            };
        }
    }
}
