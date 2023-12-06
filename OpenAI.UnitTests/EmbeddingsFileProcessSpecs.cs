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
    public class EmbeddingsFileProcessSpecs
    {
        OpenAiAPIRepository OpenAiRepo;
        const string RawDataSet = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\OpenAiDataset.csv";
        const string OutputDataSet = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv";
        public EmbeddingsFileProcessSpecs()
        {
            Config.Init("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");
            OpenAiRepo = new OpenAiAPIRepository();
        }

        [TestMethod]
        public void SetEmbeddings_ToCSVfile_ThenSave()
        {
            var lines = File.ReadAllLines(RawDataSet);
            var dataRecords = lines.Skip(1).Select(line =>
            {
                var columns = line.Split(',');
                return new EmbeddingCSVdataRecords
                {
                    Id = columns[0],
                    Text = columns[1]
                };
            }).ToList();


            List<string> stringsToEmbed = new List<string>();
            foreach(var data in dataRecords)
            {
                stringsToEmbed.Add(data.Text);
            }

            EmbeddingRequestDTO requestDTO = new EmbeddingRequestDTO();
            EmbeddingResponseDTO response = new EmbeddingResponseDTO();
            requestDTO.Model = "text-embedding-ada-002";
            requestDTO.Input = stringsToEmbed;
            Task.Run(async () =>
            {
                response = await OpenAiRepo.CreateEmbeddings(requestDTO);
            }).GetAwaiter().GetResult();

            var csv = new StringBuilder();
            csv.AppendLine("text|embedding");
            int responseCtr = 0;
            foreach (var row in dataRecords)
            {                       
                string embeddings = JsonSerializer.Serialize( response.Data[responseCtr].Embedding);
                var formattedStr = row.Text.Replace("|", "");
                csv.AppendLine(formattedStr + "|" + embeddings);
                responseCtr++;
            }
            File.WriteAllText(OutputDataSet, csv.ToString());
        }

        [TestMethod]
        public void GetEmbeddings_FromCSVfile()
        {
            var lines = File.ReadAllLines(OutputDataSet);
            var dataRecords = lines.Skip(1).Select(line =>
            {
                var columns = line.Split("|");
                return new EmbeddingCSVDataFrame
                {
                    text = columns[0],
                    embedding = JsonSerializer.Deserialize<List<float>>(columns[1])
                };
            }).AsQueryable();
        }
    }
}
