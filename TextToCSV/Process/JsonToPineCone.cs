using Newtonsoft.Json;
using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.OpenAiRepository.Model;
using OpenAiCore.OpenAiRepository.Model.JsonModels;
using OpenAiCore.PineConeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToCSV.Process
{
    internal class JsonToPineCone
    {
        PineConeRepository _pineConeRepository;
        OpenAiAPIRepository _openAiRepository;
        internal JsonToPineCone()
        {
            _pineConeRepository = new PineConeRepository();
            _openAiRepository = new OpenAiAPIRepository();
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "1000f1c9-9a38-471a-bdc5-483957668b0d");

        }
        public async Task main()
        {
            string jsonFilePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\ContextArticleEmbedded.json";
            var jsonDataSet = File.ReadAllText(jsonFilePath);

            var embeddedData = JsonConvert.DeserializeObject<EmbeddedJsonData>(jsonDataSet);

            // Save Embeddings to PineCone
            PineConeUpsertRequestDTO requestBody = new PineConeUpsertRequestDTO()
            {
                Vectors = new List<PineConeVectorsDTO>(),
                Namespace = "ChatBotApp"
            };
            

            foreach (var data in embeddedData.EmbeddedData)
            {
                PineConeVectorsDTO vector = new PineConeVectorsDTO()
                {
                    ID = Guid.NewGuid().ToString(),
                    Values = data.embedding,
                    Metadata = new PineConeMetaDataDTO() { Url = data.url, Text = data.text }
                };

                requestBody.Vectors.Add(vector);
            }

            var response = await _pineConeRepository.Upsert(requestBody);

            // Delete Embeddedjson Data
            jsonDataSet = JsonConvert.SerializeObject(new EmbeddedJsonData());
            File.WriteAllText(jsonFilePath, jsonDataSet);
        }
    }
}
