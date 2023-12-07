using MathNet.Numerics;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO;
using OpenAiCore.OpenAiRepository.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAiCore.OpenAiServices
{
    public class OpenAiService
    {
        OpenAiAPIRepository OpenAiRepo;
        public OpenAiService() {
            OpenAiRepo = new OpenAiAPIRepository();
        }

        private async Task<List<float>> GetQueryEmbeddingsAsync(string query)
        {
            EmbeddingRequestDTO EmbeddingrequestBody = new EmbeddingRequestDTO();
            EmbeddingResponseDTO Embeddingresponse = new EmbeddingResponseDTO();
            EmbeddingrequestBody.Model = "text-embedding-ada-002";
            EmbeddingrequestBody.Input = new List<string>()
            {
                query
            };

            Embeddingresponse = await OpenAiRepo.CreateEmbeddings(EmbeddingrequestBody);
            return Embeddingresponse.Data[0].Embedding;
        }

        private List<EmbeddingCSVDataFrame> GetEmbeddedCSVData()
        {
            var lines = File.ReadAllLines(Config.OutputDataSet);
            var dataRecords = lines.Skip(1).Select(line  =>
            {
                var columns = line.Split('|');
                return new EmbeddingCSVDataFrame
                {
                    url = columns[0],
                    text = columns[1],
                    embedding = JsonSerializer.Deserialize<List<float>>(columns[2])
                };
            }).ToList();

            return dataRecords;
        }

        private List<EmbeddingRanking> GetRankings(List<EmbeddingCSVDataFrame> dataRecords, List<float> QueryEmbeddings)
        {
            List<EmbeddingRanking> rankings = new List<EmbeddingRanking>();

            foreach (var row in dataRecords)
            {
                var relatedness = Distance.Cosine(QueryEmbeddings.ToArray(), row.embedding.ToArray());
                rankings.Add(new EmbeddingRanking() { Relatedness = relatedness, Percent = (100 - (relatedness * 100)).ToString(), Text = row.text + ", sources: " + row.url });
            }

            var topRank = rankings.OrderBy(x => x.Relatedness).Take(20).ToList();

            return topRank;
        }

        public async Task<ChatCompletionResponseDTO> GetChatCompletion_withSearch(ChatCompletionRequestDTO body, string userMessage)
        {
            var QueryEmbeddings = await GetQueryEmbeddingsAsync(userMessage);
            var CsvRecords = GetEmbeddedCSVData();
            var Rankings = GetRankings(CsvRecords, QueryEmbeddings);

            string QueryMessage = "Use the below article to answer the question and write the url sources of the article at the end of your answer. Only based your answer on the article below provided. Use the Answer format below" +
                " Answer Format:  " + 
                " \" Answer \" \n " +
                " \"For more info you can check our Sources: <a url=\"https://urlhere\">1</a> \" \n" + 
                "Article: \"\"\"";
            foreach(var i in Rankings)
            {
                QueryMessage = QueryMessage + i.Text;
            }

            QueryMessage = QueryMessage + "\"\"\"";
            QueryMessage = QueryMessage + "Question : " + userMessage;

            body.Messages.Add(new MessagesDTO() { Role = "user", Content = QueryMessage });
            var response = await OpenAiRepo.ChatCompletion(body);

            return response;
        }
    }
}
