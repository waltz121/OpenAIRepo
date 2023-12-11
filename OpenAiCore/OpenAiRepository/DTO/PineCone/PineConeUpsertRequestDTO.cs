using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeUpsertRequestDTO
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("values")]
        public List<float> Values { get; set; }

        [JsonPropertyName("metadata")]
        public List<KeyValuePair<string, string>> Metadata { get; set; }
    }
}
