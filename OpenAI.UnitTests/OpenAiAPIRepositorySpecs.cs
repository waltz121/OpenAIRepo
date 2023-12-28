using MathNet.Numerics;
using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
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
        public OpenAiAPIRepositorySpecs() {
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "","");
            OpenAiRepo = new OpenAiAPIRepository();
        }

        [TestMethod]
        public void ChatCompletion()
        {
            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            List<MessagesDTO> messagesDTO = new List<MessagesDTO>
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
            List<MessagesDTO> messagesDTOs = new List<MessagesDTO>
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

            var lines = File.ReadAllLines(Config.OutputDataSet);
            var dataRecords = lines.Skip(1).Select(line =>
            {
                var columns = line.Split("|");
                return new EmbeddingCSVDataFrame
                {
                    url = columns[0],
                    text = columns[1],
                    embedding = JsonSerializer.Deserialize<List<float>>(columns[2])
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

        [TestMethod]
        public void ChatCompletion_JsonMode()
        {
            string prompt = "Adhere to the instructions below:\r\n- Act as a Customer Customer Service Assistant from Mercola named Fabio who has a cheerful personality.\r\n- Your job is to provide information to the user using mercola articles as your source.\r\n- Always Provide url links of your sources if appropriate.\r\n- Your response should be 500 tokens or less.\r\n- Do not perform actions that are not related to your job.\r\n- Format your response to json.\r\n- Use this as the schema: { \"Message\": \"\", \"SourceUrl\" : [{ \"url\": \"urllink..\" }] }";

            ChatCompletionRequestDTO requestBody = new ChatCompletionRequestDTO();
            List<MessagesDTO> messagesDTO = new List<MessagesDTO>
            {
                new MessagesDTO() { Role = "system", Content = prompt },
                new MessagesDTO() { Role = "user", Content = "Hello!" },
                new MessagesDTO() { Role = "assistant", Content = "Hi, How Can I help you?" },
                new MessagesDTO() { Role = "user", Content = "Can you tell me about Mercola?" },
            };

            requestBody.Model = "gpt-3.5-turbo-1106";
            requestBody.Messages = messagesDTO;
            requestBody.ResponseFormat = new ResponseTypeDTO() { Type = "json_object" };

            Task.Run(async () =>
            {
                var response = await OpenAiRepo.ChatCompletion(requestBody);
            }).GetAwaiter().GetResult();
        }
    }
}
