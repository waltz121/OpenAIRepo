using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.PineConeRepository;
using OpenAiCore.Shared;
using OpenAiCore.SQLRepository;
using OpenAiCore.SQLRepository.DTO;
using System.Text.Json;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class SQLDataRepositorySpecs
    {
        SQLDataRepository sqlDataRepository;
        DataPipelineCoreUtility dataPipelineCoreUtility;
        StringCodeUtility codeUtility;
        OpenAiAPIRepository openAiAPIRepository;
        PineConeRepository pineConeRepository;
        public SQLDataRepositorySpecs()
        {
            // Personal Stack
            Config.Init("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",
                "",
                "1000f1c9-9a38-471a-bdc5-483957668b0d",
                "Data Source=PRODDBSOCIAL.NEWMERCOLA.COM;Initial Catalog=CommunityServer;User ID=webuser_communityserver;Password=w3bu$3r_c0mmun1ty$3rv3r!;Encrypt=False",
                "https://embeddeddataset-ps4dv8t.svc.gcp-starter.pinecone.io",
                "MercolaDataset");

            // OpenAi.PineCone Stack
            //Config.Init("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",
            //"",
            //"08d21b42-5fa8-436e-8160-c89c8575b4dd",
            //"Data Source=PRODDBSOCIAL.NEWMERCOLA.COM;Initial Catalog=CommunityServer;User ID=webuser_communityserver;Password=w3bu$3r_c0mmun1ty$3rv3r!;Encrypt=False",
            //"https://mercoladataset-cbac0kl.svc.gcp-starter.pinecone.io",
            //"MercolaDataset");

            sqlDataRepository = new SQLDataRepository();
            codeUtility = new StringCodeUtility();
            openAiAPIRepository = new OpenAiAPIRepository();
            pineConeRepository = new PineConeRepository();
            dataPipelineCoreUtility = new DataPipelineCoreUtility(pineConeRepository, openAiAPIRepository, codeUtility);
        }

        [TestMethod]
        public void GetArticlesForProcessing_InSQL()
        {
            //Code to get string value on a file
            string sql = File.ReadAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\SQLPostArticlequery.txt");
            var ArticlesForProcessing = sqlDataRepository.GetArticlesForProcessing(sql);
        }

        [TestMethod]
        public void SQLtoJsonPipeline()
        {
            string sql = File.ReadAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\SQLPostArticlequery.txt");
            var ArticlesForProcessing = sqlDataRepository.GetArticlesForProcessing(sql);
            List<EmbeddedArticlesDTO> embeddedArticlesDTOs = dataPipelineCoreUtility.checkJsonForExistingFile();
            dataPipelineCoreUtility.CheckAndSetEmbeddedArticles(ArticlesForProcessing, embeddedArticlesDTOs);
            dataPipelineCoreUtility.SqlToJsonProcess(embeddedArticlesDTOs);
        }

        [TestMethod]
        public void JsonToPineConePipeline()
        {
            dataPipelineCoreUtility.JsonToPineConeProcess(dataPipelineCoreUtility.checkJsonForExistingFile());
        }

        [TestMethod]
        public void SQLtoPineConeDataPipeline()
        {
            string sql = File.ReadAllText(@"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\SQLPostArticlequery.txt");
            var ArticlesForProcessing = sqlDataRepository.GetArticlesForProcessing(sql);
            List<EmbeddedArticlesDTO> embeddedArticlesDTOs = dataPipelineCoreUtility.checkJsonForExistingFile();
            dataPipelineCoreUtility.CheckAndSetEmbeddedArticles(ArticlesForProcessing, embeddedArticlesDTOs);
            bool continueBatchProcessing = dataPipelineCoreUtility.CheckForBatchStatus(embeddedArticlesDTOs);

            while (continueBatchProcessing)
            {
                dataPipelineCoreUtility.SqlToJsonProcess(embeddedArticlesDTOs);

                //Code to wait for 1 minute
                //Task.Delay(60000).Wait();

                dataPipelineCoreUtility.JsonToPineConeProcess(embeddedArticlesDTOs);
                continueBatchProcessing = dataPipelineCoreUtility.CheckForBatchStatus(embeddedArticlesDTOs);

                //Code to wait for 30 seconds
                //Task.Delay(30000).Wait();
            }
        }
    }
}
