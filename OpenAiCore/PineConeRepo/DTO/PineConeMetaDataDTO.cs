using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeMetaDataDTO
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        
        [JsonPropertyName("splitType")]
        public string SplitType { get; set; }
        
        [JsonPropertyName("batchStatus")]
        public string BatchStatus { get; set; }
    }
}