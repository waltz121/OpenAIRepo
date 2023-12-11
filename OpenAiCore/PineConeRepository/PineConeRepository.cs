using OpenAiCore.OpenAiRepository.DTO.PineCone;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.Pinecone_ApiKey);
            var url = _baseUrl + "/vectors/upsert";
            var jsonBody = JsonSerializer.Serialize(requestDTO);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(url, content);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Error calling PineCone Upsert");
            }
            else
            {
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseDTO = JsonSerializer.Deserialize<PineConeUpsertResponseDTO>(responseContent);
                return responseDTO;
            }
        }
    }
}
