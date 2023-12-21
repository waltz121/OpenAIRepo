using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeQueryFilterDTO
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}