using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.OpenAi
{
    public class ResponseTypeDTO
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}