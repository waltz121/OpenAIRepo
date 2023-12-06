using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore
{
    public static class Config
    {
        public static string OpenAI_ApiKey { get; private set; }
        public static string OutputDataSet { get; private set; }

        public static void Init(string apiKey,string outputDataSet)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                OpenAI_ApiKey = apiKey;
            }

            if (outputDataSet == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                OutputDataSet = outputDataSet;
            }
        }

        public static string IsInitialized()
        {
            if (string.IsNullOrEmpty(OpenAI_ApiKey))
            {
                return "Open Api Key is Null or Empty";
            }

            if (string.IsNullOrEmpty(OutputDataSet))
            {
                return "OutputDataSet is null or empty";
            }

            return string.Empty;
        }
    }
}
