using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeVectorsDTO
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("values")]
        public List<float> Values { get; set; }

        [JsonPropertyName("metadata")]
        public PineConeMetaDataDTO Metadata { get; set; }
    }
}
