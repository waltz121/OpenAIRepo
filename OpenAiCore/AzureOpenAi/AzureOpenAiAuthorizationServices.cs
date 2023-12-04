using OpenAI_API;
using OpenAiCore.AzureOpenAi.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore.AzureOpenAi
{
    public class AzureOpenAiAuthorizationServices : IAzureOpenAiAuthorizationServices
    {
        private string ConfigReturnMsg = "";
        public AzureOpenAiAuthorizationServices() {
            ConfigReturnMsg = Config.IsInitialized();
        }

        public OpenAIAPI FromAzure()
        {
            throw new NotImplementedException();
        }

        public OpenAIAPI FromOpenAiOrg()
        {
            OpenAIAPI api = new OpenAIAPI(Config.OpenAI_ApiKey);
            return api;
        }


    }
}
