using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.SignalR;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.PineConeRepository;
using OpenAiCore.Shared;
using OpenAiCore.SQLRepository;
using OpenAiCore.SQLRepository.DTO;

namespace ChatBot.Hubs
{
    public class BatchUploadHub : Hub
    {
        SQLDataRepository _sqlDataRepository;
        PineConeRepository _pineConeRepository;
        StringCodeUtility _stringCodeUtility;
        OpenAiAPIRepository _openAiAPIRepository;
        DataPipelineCoreUtility _dataPipelineCoreUtility;
        public BatchUploadHub()
        {
            _sqlDataRepository = new SQLDataRepository();
            _pineConeRepository = new PineConeRepository();
            _stringCodeUtility = new StringCodeUtility();
            _openAiAPIRepository = new OpenAiAPIRepository();
            _dataPipelineCoreUtility = new DataPipelineCoreUtility(_pineConeRepository, _openAiAPIRepository, _stringCodeUtility);
            _dataPipelineCoreUtility.SetEmbedDataFilePath(setDynamicPath() + "MercolaEmbeddedDataset.json");
        }

        private string setDynamicPath()
        {
            return @"wwwroot\Dataset\";
        }

        public async Task SubmitSqlStatement(string sqlStatement)
        {
            var ArticlesForProcessing = _sqlDataRepository.GetArticlesForProcessing(sqlStatement);
            List<EmbeddedArticlesDTO> embeddedArticlesDTOs = _dataPipelineCoreUtility.checkJsonForExistingFile();
            _dataPipelineCoreUtility.CheckAndSetEmbeddedArticles(ArticlesForProcessing, embeddedArticlesDTOs);
            bool continueBatchProcessing = _dataPipelineCoreUtility.CheckForBatchStatus(embeddedArticlesDTOs);

            while (continueBatchProcessing)
            {
                await _dataPipelineCoreUtility.SqlToJsonProcessAsync(embeddedArticlesDTOs);

                await _dataPipelineCoreUtility.JsonToPineConeProcessAsync(embeddedArticlesDTOs);
                await Clients.All.SendAsync("UpdateProgressBar", GetProgress(embeddedArticlesDTOs));
                continueBatchProcessing = _dataPipelineCoreUtility.CheckForBatchStatus(embeddedArticlesDTOs);
            }
            await Clients.All.SendAsync("UpdateProgressBar", GetProgress(embeddedArticlesDTOs));
            CheckAndResetAllCompletedArticles(embeddedArticlesDTOs);
        }

        private void CheckAndResetAllCompletedArticles(List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            var total = embeddedArticlesDTOs.Count;
            var completed = embeddedArticlesDTOs.Where(x => x.BatchStatus == "Completed").Count();
            if (total == completed)
            {
                _dataPipelineCoreUtility.DeleteDataFilePath();
            }
        }

        private double GetProgress(List<EmbeddedArticlesDTO> embeddedArticlesDTOs)
        {
            double total = embeddedArticlesDTOs.Count;
            double completed = embeddedArticlesDTOs.Where(x => x.BatchStatus == "Completed").Count();
            double progress = (completed / total) * 100;
            return progress;
        }
    }
}
