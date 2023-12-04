using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore.AzureOpenAi.Interface
{
    public interface IAzureOpenAiAuthorizationServices
    {
        OpenAIAPI FromOpenAiOrg();
        OpenAIAPI FromAzure();
    }
}
