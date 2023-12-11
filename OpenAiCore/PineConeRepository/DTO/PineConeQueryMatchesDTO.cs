using OpenAiCore.OpenAiRepository.DTO.PineCone;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenAiCore.PineConeRepository.DTO
{
    public class PineConeQueryMatchesDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }

        [JsonPropertyName("values")]
        public List<float> Values { get; set; }

        [JsonPropertyName("metadata")]
        public PineConeMetaDataDTO Metadata { get; set; }

    }
}
