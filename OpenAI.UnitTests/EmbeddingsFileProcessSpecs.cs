using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiRepository.Model;
using OpenAiCore.OpenAiRepository.Model.JsonModels;
using System.Text;
using System.Text.Json;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class EmbeddingsFileProcessSpecs
    {
        OpenAiAPIRepository OpenAiRepo;
        const string RawDataSet = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\OpenAiDataset.csv";
        const string JsonDataSet = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\OpenAiJsonDataSet.json";
        const string OutputJsonDataSet = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\OpenAiEmbeddedJsonDataSet.csv";

        public EmbeddingsFileProcessSpecs()
        {
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "", "", "https://mercoladataset-cbac0kl.svc.gcp-starter.pinecone.io");
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
            foreach (var data in dataRecords)
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
                string embeddings = JsonSerializer.Serialize(response.Data[responseCtr].Embedding);
                var formattedStr = row.Text.Replace("|", "");
                csv.AppendLine(formattedStr + "|" + embeddings);
                responseCtr++;
            }
            File.WriteAllText(Config.OutputDataSet, csv.ToString());
        }

        [TestMethod]
        public void GetEmbeddings_FromCSVfile()
        {
            var lines = File.ReadAllLines(Config.OutputDataSet);
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

        [TestMethod]
        public void GetJson_SaveCSVFile()
        {
            string jsonContent = File.ReadAllText(JsonDataSet);
            JsonDataFrame jsonData = JsonSerializer.Deserialize<JsonDataFrame>(jsonContent);
            List<string> csvContent = new List<string>();
            var csv = new StringBuilder();
            csv.AppendLine("URL,TEXT");
            int CharLimit = 120;
            int CharCounter = 0;
            string tmpStr = "";
            foreach (var post in jsonData.root.posts)
            {
                foreach (var character in post.content)
                {
                    tmpStr = tmpStr + character;
                    if (CharCounter >= CharLimit)
                    {
                        if (character == '.' || character == '?' || character == '!')
                        {
                            var formattedStr = tmpStr.Replace(",", "");
                            csv.AppendLine(post.url + "," + formattedStr);
                            csvContent.Add(tmpStr);
                            tmpStr = "";
                            CharCounter = 0;
                        }
                    }
                    CharCounter++;
                }
            }

            File.WriteAllText(OutputJsonDataSet, csv.ToString());
        }

        [TestMethod]
        public void SetEmbeddings_SaveWithURL()
        {
            var lines = File.ReadAllLines(OutputJsonDataSet);
            var dataRecords = lines.Skip(1).Select(line =>
            {
                var columns = line.Split(',');
                return new EmbeddingCSVdataRecords
                {
                    Url = columns[0],
                    Text = columns[1]
                };
            }).ToList();

            List<string> stringsToEmbed = new List<string>();
            foreach (var data in dataRecords)
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
            csv.AppendLine("url|text|embedding");
            int responseCtr = 0;
            foreach (var row in dataRecords)
            {
                string embeddings = JsonSerializer.Serialize(response.Data[responseCtr].Embedding);
                var formattedStr = row.Text.Replace("|", "");
                csv.AppendLine(row.Url + "|" + formattedStr + "|" + embeddings);
                responseCtr++;
            }
            File.WriteAllText(Config.OutputDataSet, csv.ToString());
        }
    }
}