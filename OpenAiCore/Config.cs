using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore
{
    public static class Config
    {
        public static string OpenAI_ApiKey { get; private set; }

        public static void Init(string apiKey)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                OpenAI_ApiKey = apiKey;
            }
        }

        public static string IsInitialized()
        {
            if (string.IsNullOrEmpty(OpenAI_ApiKey))
            {
                return "Open Api Key is Null or Empty";
            }

            return string.Empty;
        }
    }
}
