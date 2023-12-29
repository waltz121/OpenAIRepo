using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore
{
    public static class Config
    {
        public static string OpenAI_ApiKey { get; private set; }
        public static string OutputDataSet { get; private set; }
        public static string Pinecone_ApiKey { get; private set; }
        public static string Pinecone_BaseUrl { get; private set; }
        public static string SQLConnectionString { get; private set; }

        public static void Init(string apiKey,string outputDataSet, string pineconeApikey, string sqlConnString, string pineconeBaseUrl)
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

            if(pineconeApikey == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                Pinecone_ApiKey = pineconeApikey;
            }

            if(sqlConnString == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                SQLConnectionString = sqlConnString;
            }

            if(pineconeBaseUrl == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                Pinecone_BaseUrl = pineconeBaseUrl;
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

            if (string.IsNullOrEmpty(Pinecone_ApiKey))
            {
                return "Pinecone Api Key is null or empty";
            }

            if (string.IsNullOrEmpty(SQLConnectionString))
            {
                return "SQL Connection String is null or empty";
            }

            if (string.IsNullOrEmpty(Pinecone_BaseUrl))
            {
                return "Pinecone Base Url is null or empty";
            }
            

            return string.Empty;
        }
    }
}
