using MathNet.Numerics;
using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO;
using OpenAiCore.OpenAiRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class OpenAiAPIRepositorySpecs
    {
        OpenAiAPIRepository OpenAiRepo;
        const string OutputDataSet = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv";
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

        [TestMethod]
        public void GetRelatednessFromCSVFile_To_SearchQuery()
        {
            EmbeddingRequestDTO requestBody = new EmbeddingRequestDTO();
            EmbeddingResponseDTO response = new EmbeddingResponseDTO();
            requestBody.Model = "text-embedding-ada-002";
            requestBody.Input = new List<string>()
            {
                "What does having low vitamin D Means?"
            };

            Task.Run(async () =>
            {
                response = await OpenAiRepo.CreateEmbeddings(requestBody);
            }).GetAwaiter().GetResult();

            var QueryEmbeddings = response.Data[0].Embedding;

            var lines = File.ReadAllLines(OutputDataSet);
            var dataRecords = lines.Skip(1).Select(line =>
            {
                var columns = line.Split("|");
                return new EmbeddingCSVDataFrame
                {
                    text = columns[0],
                    embedding = JsonSerializer.Deserialize<List<float>>(columns[1])
                };
            }).ToList();
            
            List<EmbeddingRanking> rankings = new List<EmbeddingRanking>();

            foreach(var row in dataRecords)
            {                
                var relatedness = Distance.Cosine(QueryEmbeddings.ToArray(), row.embedding.ToArray());
                rankings.Add(new EmbeddingRanking() { Relatedness = relatedness, Percent = (100 - (relatedness * 100)).ToString(), Text = row.text  });
            }

            var topRank = rankings.OrderBy(x => x.Relatedness).Take(5).ToList();
            
        }
    }
}
