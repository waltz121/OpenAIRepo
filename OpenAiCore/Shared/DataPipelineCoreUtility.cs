using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.PineConeRepository;
using OpenAiCore.SQLRepository.DTO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAiCore.Shared
{
    public class DataPipelineCoreUtility
    {
        StringCodeUtility codeUtility;
        PineConeRepository.PineConeRepository pineConeRepository;
        OpenAiAPIRepository openAiAPIRepository;
        public DataPipelineCoreUtility(PineConeRepository.PineConeRepository _pineconeRepo, OpenAiAPIRepository openAiAPIRepository, StringCodeUtility stringCodeUtility)
        {
            codeUtility = stringCodeUtility;
            pineConeRepository = _pineconeRepo;
            this.openAiAPIRepository = openAiAPIRepository;
        }
        public bool CheckForBatchStatus(List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            bool continueBatchProcessing;
            // Code for Checking if there are pending embedded articles or embedded status
            var pendingEmbeddedArticles = embeddedArticlesDTOs.Where(x => x.BatchStatus == "Pending").ToList();
            var embeddingEmbeddedArticles = embeddedArticlesDTOs.Where(x => x.BatchStatus == "Embedding").ToList();

            if (pendingEmbeddedArticles.Count == 0 && embeddingEmbeddedArticles.Count == 0)
            {
                continueBatchProcessing = false;
            }
            else
            {
                continueBatchProcessing = true;
            }

            return continueBatchProcessing;
        }

        public void CheckAndSetEmbeddedArticles(List<ArticlesForProcessingDTO> articlesForProcessing, List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            if (embeddedArticlesDTOs.Count == 0)
            {
                foreach (var articles in articlesForProcessing)
                {
                    var htmlInput = codeUtility.RemoveScriptsStylesAndHtml(articles.FormattedBody);

                    List<string> textList = codeUtility.SplitTextToLimit(300, htmlInput);
                    EmbeddedArticlesDTO embeddedArticles = new EmbeddedArticlesDTO()
                    {
                        texts = textList,
                        url = articles.SubstackURL,
                        splitType = "300words",
                        PostID = articles.PostID,
                        BatchStatus = "Pending"
                    };
                    embeddedArticlesDTOs.Add(embeddedArticles);
                }
            }
        }

        public List<EmbeddedArticlesDTO> checkJsonForExistingFile()
        {

            //Check if file is existing
            if (File.Exists(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\MercolaEmbeddedDataset.json"))
            {
                //Code for getting input from a json file
                string jsonString = File.ReadAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\MercolaEmbeddedDataset.json");
                if (jsonString != "")
                {
                    var embeddedArticlesDTOs = JsonSerializer.Deserialize<List<EmbeddedArticlesDTO>>(jsonString);
                    return embeddedArticlesDTOs;
                }
                else
                {
                    return new List<EmbeddedArticlesDTO>();
                }
            }
            else
            {
                return new List<EmbeddedArticlesDTO>();
            }
        }

        public string GenerateVectorID(EmbeddedArticlesDTO embeddedArticles, int index)
        {
            string idstr = "";
            if (embeddedArticles.url.Contains("substack"))
            {
                idstr = idstr + "substack-";
            }

            idstr = idstr + embeddedArticles.PostID + "-" + embeddedArticles.splitType + "-" + index.ToString();

            return idstr;
        }

        public string GenerateVectorID(string Domain, string PostID, string splitType, int index)
        {
            string idstr = "";

            return idstr + Domain + "-" + PostID + "-" + splitType + "-" + index.ToString();
        }

        public void JsonToPineConeProcess(List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            if (embeddedArticlesDTOs.Count > 0)
            {
                PineConeUpsertRequestDTO PineConerequestDto = new PineConeUpsertRequestDTO();
                PineConerequestDto.Vectors = new List<PineConeVectorsDTO>();
                PineConerequestDto.Namespace = "MercolaDataset";

                foreach (var embeddedArticle in embeddedArticlesDTOs)
                {
                    if (embeddedArticle.Embeddings != null && embeddedArticle.BatchStatus == "Embedded")
                    {
                        foreach (var embbeds in embeddedArticle.Embeddings)
                        {
                            PineConeVectorsDTO pineConeVectors = new PineConeVectorsDTO();
                            var embededIndex = embeddedArticle.Embeddings.IndexOf(embbeds);
                            pineConeVectors.ID = GenerateVectorID(embeddedArticle, embededIndex);
                            pineConeVectors.Values = embbeds;
                            pineConeVectors.Metadata = new PineConeMetaDataDTO() { Url = embeddedArticle.url, Text = embeddedArticle.texts[embededIndex], SplitType = embeddedArticle.splitType, BatchStatus = "Completed" };
                            PineConerequestDto.Vectors.Add(pineConeVectors);
                        }
                        embeddedArticle.BatchStatus = "Completed";
                    }
                }

                Task.Run(async () =>
                {
                    if (PineConerequestDto.Vectors.Count != 0)
                    {
                        var response = await pineConeRepository.Upsert(PineConerequestDto);
                        SaveToJsonFile(embeddedArticlesDTOs);
                    }

                }).GetAwaiter().GetResult();
            }
        }

        public void SaveToJsonFile(List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            //Code for converting class to Json
            var json = JsonSerializer.Serialize(embeddedArticlesDTOs);
            File.WriteAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\MercolaEmbeddedDataset.json", json);
        }

        public void SqlToJsonProcess(List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            int embeddedarticlesCtr = 0;
            foreach (var embeddedArticles in embeddedArticlesDTOs)
            {
                int index = embeddedArticlesDTOs.IndexOf(embeddedArticles);
                EmbeddingRequestDTO embedRequest = new EmbeddingRequestDTO();
                EmbeddingResponseDTO embedResponse = new EmbeddingResponseDTO();
                embedRequest.Model = "text-embedding-ada-002";
                embedRequest.Input = embeddedArticles.texts;

                if (embeddedArticles.Embeddings == null && embeddedArticles.BatchStatus == "Pending")
                {
                    Task.Run(async () =>
                    {
                        embedResponse = await openAiAPIRepository.CreateEmbeddings(embedRequest);
                    }).GetAwaiter().GetResult();

                    embeddedArticles.Embeddings = new List<List<float>>();

                    foreach (var data in embedResponse.Data)
                    {
                        var EmbededIndex = embedResponse.Data.IndexOf(data);
                        embeddedArticles.Embeddings.Add(data.Embedding);
                        embeddedArticles.BatchStatus = "Embedded";
                    }
                    embeddedarticlesCtr++;
                }

                if (embeddedarticlesCtr == 3)
                {
                    SaveToJsonFile(embeddedArticlesDTOs);
                    break;
                }

                //Code for checking if index is last
                if (index == embeddedArticlesDTOs.Count - 1)
                {
                    SaveToJsonFile(embeddedArticlesDTOs);
                    break;
                }
            }
        }
    }
}