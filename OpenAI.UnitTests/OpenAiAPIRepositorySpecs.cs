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
        public void ChatCompletion_LongReply()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            MessagesDTO[] messagesDTOs = new MessagesDTO[]
            {
                new MessagesDTO() { Role = "system", Content = "You are a helpful Customer Service Assistant from Mercola Named Fabio. You greet the user and asks how you can assist them.As a Customer Service Assistant you have a cheerful and joyful personality it shows on your reply.Answer as clear, concise and succint as possible." },
                new MessagesDTO() { Role = "assistant", Content = "Hello! How can I assist you today?" },
                new MessagesDTO() { Role = "user", Content = "Can you tell me about Mercola?" },
                new MessagesDTO() { Role = "assistant", Content = "Certainly! Mercola is a health and wellness company founded by Dr. Joseph Mercola. We are dedicated to providing high-quality products and information to help people lead healthier lives. Our products range from supplements to personal care items, all made with natural ingredients. We also offer a wealth of health-related articles and resources on our website. If you have any specific questions or need assistance with our products or services, feel free to ask!" },
                new MessagesDTO() { Role = "user", Content = "What products you can suggest?" }
            };

            requestBody.Model = "gpt-3.5-turbo";
            requestBody.Messages = messagesDTOs;
            requestBody.MaxTokens = 400;
            Task.Run(async () =>
            {
                var response = await OpenAiRepo.ChatCompletion(requestBody);
            }).GetAwaiter().GetResult();
        }
        
        [TestMethod]
        public void Create_SimpleEmbeddingsSample()
        {
            EmbeddingRequestDTO requestBody = new EmbeddingRequestDTO();
            requestBody.Model = "text-embedding-ada-002";
            requestBody.Input = new List<string>()
            {
                "Quick brown fox jumps over the lazy dog"
            };

            Task.Run(async () =>
            {
                var response = await OpenAiRepo.CreateEmbeddings(requestBody);
            }).GetAwaiter().GetResult();
        }
    }
}
