using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.PineConeRepository.DTO;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAiCore.PineConeRepository
{
    public class PineConeRepository
    {
        private readonly HttpClient httpClient;
        private const string _baseUrl = "https://embeddeddataset-ps4dv8t.svc.gcp-starter.pinecone.io";
        public PineConeRepository()
        {
            httpClient = new HttpClient();
        }

        public async Task<PineConeUpsertResponseDTO> Upsert(PineConeUpsertRequestDTO requestDTO)
        {

            httpClient.DefaultRequestHeaders.Add("Api-Key", Config.Pinecone_ApiKey);
            var url = _baseUrl + "/vectors/upsert";
            var jsonBody = JsonSerializer.Serialize(requestDTO);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(url, content);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error calling PineCone Upsert");
            }
            else
            {

                var responseDTO = JsonSerializer.Deserialize<PineConeUpsertResponseDTO>(responseContent);
                return responseDTO;
            }
        }

        public async Task<PineConeQueryResponseDTO> Query(PineConeQueryRequestDTO requestDTO)
        {
            httpClient.DefaultRequestHeaders.Add("Api-Key", Config.Pinecone_ApiKey);
            var url = _baseUrl + "/query";
            var jsonBody = JsonSerializer.Serialize(requestDTO);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(url, content);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error calling PineCone Query");
            }
            else
            {
                var responseDTO = JsonSerializer.Deserialize<PineConeQueryResponseDTO>(responseContent);
                return responseDTO;
            }
        }        
    }
}
