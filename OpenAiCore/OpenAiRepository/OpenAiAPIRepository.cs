using OpenAiCore.OpenAiRepository.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAiCore.OpenAiRepository
{
    public class OpenAiAPIRepository
    {
        private const string _baseURL = "https://api.openai.com/v1";
        public OpenAiAPIRepository() { 
            
        }

        public async Task<ChatCompletionResponseDTO> ChatCompletion(ChatCompletionRequestDTO body)
        {
            var url = _baseURL + "/chat/completions";
            var token = Config.OpenAI_ApiKey;
            ChatCompletionResponseDTO responseDTO;
            using (var client = new HttpClient())
            {
               
                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.OpenAI_ApiKey);
                HttpResponseMessage httpResponseMessage = await client.PostAsync(url, content);

                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    responseDTO = JsonSerializer.Deserialize<ChatCompletionResponseDTO>(responseString);
                }
                else
                {
                    responseDTO = new ChatCompletionResponseDTO();
                }
            }

            return responseDTO;
        }

        public async Task<EmbeddingResponseDTO> CreateEmbeddings(EmbeddingRequestDTO requestDTO)
        {
            var url = _baseURL + "/embeddings";
            var token = Config.OpenAI_ApiKey;
            EmbeddingResponseDTO responseDTO;
            
            using (var client = new HttpClient())
            {
                var jsonBody = JsonSerializer.Serialize(requestDTO);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.OpenAI_ApiKey);
                HttpResponseMessage httpResponseMessage = await client.PostAsync(url, content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    responseDTO = JsonSerializer.Deserialize<EmbeddingResponseDTO>(responseString);
                }
                else
                {
                    responseDTO = new EmbeddingResponseDTO();
                }
            }

            return responseDTO;
        }
    }
}
