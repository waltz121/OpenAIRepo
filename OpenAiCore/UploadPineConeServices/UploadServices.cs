using Newtonsoft.Json;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.OpenAiRepository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenAiCore.UploadPineConeServices
{
    public class UploadServices
    {
        private OpenAiAPIRepository openAiAPI;
        private PineConeRepository.PineConeRepository pineConeAPI;
        public UploadServices() { 
            openAiAPI = new OpenAiAPIRepository();
            pineConeAPI = new PineConeRepository.PineConeRepository();
        }
        private string GetHtmlInput(string url)
        {
            string htmlInput = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    htmlInput = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                htmlInput = "Error: " + ex.Message;
            }

            return htmlInput;
        }
        private string RemoveScriptsStylesAndHtml(string htmlInput)
        {
            // Remove javascripts from htmlInput
            htmlInput = Regex.Replace(htmlInput, "<script.*?</script>", String.Empty, RegexOptions.Singleline);

            // Remove CSS from htmlInput
            htmlInput = Regex.Replace(htmlInput, "<style.*?</style>", String.Empty, RegexOptions.Singleline);

            // Remove html tags from htmlInput
            htmlInput = Regex.Replace(htmlInput, "<.*?>", String.Empty, RegexOptions.Singleline);

            // Remove whitespaces from htmlInput
            htmlInput = Regex.Replace(htmlInput, @"\s+", " ");

            return htmlInput;
        }
        private List<string> SplitTextToLimit(int CharLimit, string Text)
        {
            int CharCounter = 0;
            string tempText = "";
            List<string> TextList = new List<string>();
            foreach (var character in Text)
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

        private async void SaveToPineCone(List<EmbeddingJsonDataFrame> EmbeddedData)
        {
            PineConeUpsertRequestDTO requestBody = new PineConeUpsertRequestDTO()
            {
                Vectors = new List<PineConeVectorsDTO>(),
                Namespace = "ChatBotApp"
            };

            foreach (var data in EmbeddedData)
            {
                PineConeVectorsDTO vector = new PineConeVectorsDTO()
                {
                    ID = Guid.NewGuid().ToString(),
                    Values = data.embedding,
                    Metadata = new PineConeMetaDataDTO() { Url = data.url, Text = data.text }
                };

                requestBody.Vectors.Add(vector);
            }

            var response = await pineConeAPI.Upsert(requestBody);

        }

        private async Task<List<EmbeddingDTO>> GetEmbeddings(List<string> TextList)
        {
            EmbeddingRequestDTO requestDTO = new EmbeddingRequestDTO();
            EmbeddingResponseDTO responseDTO = new EmbeddingResponseDTO();
            requestDTO.Model = "text-embedding-ada-002";
            requestDTO.Input = TextList;
            responseDTO = await openAiAPI.CreateEmbeddings(requestDTO);
            return responseDTO.Data;
        }

        private async Task<bool> Is_Already_OnDb(string url)
        {
            // Get Embeddings of Input
            EmbeddingRequestDTO embeddingRequestDTO = new EmbeddingRequestDTO()
            {
                Input = new List<string>()
                {
                    url
                },
                Model = "text-embedding-ada-002"
            };
            EmbeddingResponseDTO embeddingResponseDTO = new EmbeddingResponseDTO();
            embeddingResponseDTO = await openAiAPI.CreateEmbeddings(embeddingRequestDTO);

            var QueryEmbedding = embeddingResponseDTO.Data[0].Embedding;

            PineConeQueryRequestDTO requestDTO = new PineConeQueryRequestDTO()
            {
                TopK = 10,
                Namespace = "ChatBotApp",
                IncludeValues = "false",
                IncludeMetadata = "true",
                Vector = QueryEmbedding,
                Filter = new PineConeQueryFilterDTO()
                {
                    Url = url
                }
            };

            var PineConeResponse = await pineConeAPI.Query(requestDTO);

            if (PineConeResponse.Matches.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> SaveHtmlInput_To_PineCone(string url)
        {
            var htmlInput = GetHtmlInput(url);
            // Remove scripts, styles and html tags from htmlInput
            htmlInput = RemoveScriptsStylesAndHtml(htmlInput);
            
            htmlInput = htmlInput.Replace("\r\n", "");

            // Remove whitespaces from htmlInput
            htmlInput = Regex.Replace(htmlInput, @"\s+", " ");            

            List<string> textList = SplitTextToLimit(200, htmlInput);

            if(await Is_Already_OnDb(url))
            {
                return "Url is already on the database.";
            }
            else
            {
                var embeddedTexts = await GetEmbeddings(textList);
                EmbeddedJsonData embeddedJsonData = new EmbeddedJsonData();
                List<EmbeddingJsonDataFrame> EmbeddedData = new List<EmbeddingJsonDataFrame>();
                foreach (var text in embeddedTexts)
                {
                    EmbeddingJsonDataFrame dataFrame = new EmbeddingJsonDataFrame();
                    dataFrame.embedding = text.Embedding;
                    dataFrame.text = textList[embeddedTexts.IndexOf(text)];
                    dataFrame.url = url;
                    EmbeddedData.Add(dataFrame);
                }
                embeddedJsonData.EmbeddedData = EmbeddedData;
                var jsonData = JsonConvert.SerializeObject(embeddedJsonData);
                File.WriteAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\OpenAiJsonDataSet.json", jsonData);

             //   SaveToPineCone(EmbeddedData);

                return "Success";
            }         
        }


    }
}
