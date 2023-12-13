using Newtonsoft.Json;
using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.OpenAiRepository.Model;
using OpenAiCore.OpenAiRepository.Model.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToCSV.Process
{
    internal class JsonToEmbeddedJson
    {
        OpenAiAPIRepository openAiAPIRepository;
        internal JsonToEmbeddedJson()
        {
            openAiAPIRepository = new OpenAiAPIRepository();
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "1000f1c9-9a38-471a-bdc5-483957668b0d");
        }
        private async Task<List<EmbeddingDTO>> GetEmbeddings(List<string> TextList)
        {
            EmbeddingRequestDTO requestDTO = new EmbeddingRequestDTO();
            EmbeddingResponseDTO responseDTO = new EmbeddingResponseDTO();
            requestDTO.Model = "text-embedding-ada-002";
            requestDTO.Input = TextList;
            responseDTO = await openAiAPIRepository.CreateEmbeddings(requestDTO);
            return responseDTO.Data;
        }
        private List<string> SplitTextToLimit(int CharLimit, string Text)
        {
            int CharCounter = 0;
            string tempText = "";
            List<string> TextList = new List<string>();
            foreach(var character in Text)
            {
                tempText = tempText + character;
                if (CharCounter >= CharLimit)
                {
                    if (character == '.' || character == '?' || character == '!')
                    {
                        // Add to list
                        TextList.Add(tempText);
                        tempText = "";
                        CharCounter = 0;
                    }
                }
                CharCounter++;
            }
            return TextList;
        }
        
        public async Task main()
        {
            string jsonFilePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\ContextArticle.json";
            string outputJsonFilePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\ContextArticleEmbedded.json";

            // Read existing json data
            var jsonData = File.ReadAllText(jsonFilePath);
            var jsondf = JsonConvert.DeserializeObject<JsonDataFrame>(jsonData)
                                  ?? new JsonDataFrame();

            var Posts = jsondf.root.posts;
            EmbeddedJsonData embeddedJsonData = new EmbeddedJsonData();
            List<EmbeddingJsonDataFrame> EmbeddedData = new List<EmbeddingJsonDataFrame>();
            foreach (var i in Posts)
            {
                var SplitTextList = SplitTextToLimit(200, i.content);
                var Embeddings = await GetEmbeddings(SplitTextList);
                int mbeddingCtr = 0;
                foreach(var c in Embeddings)
                {
                    EmbeddingJsonDataFrame dataFrame = new EmbeddingJsonDataFrame();
                    dataFrame.embedding = c.Embedding;
                    dataFrame.text = SplitTextList[mbeddingCtr];
                    dataFrame.url = i.url;
                    EmbeddedData.Add(dataFrame);
                    mbeddingCtr++;
                }
            }
            embeddedJsonData.EmbeddedData = EmbeddedData;

            jsonData = JsonConvert.SerializeObject(embeddedJsonData);
            File.WriteAllText(outputJsonFilePath, jsonData);
        }
    }
}
