using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.OpenAi
{
    public class EmbeddingRequestDTO
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("input")]
        public List<string> Input { get; set; }

        [JsonPropertyName("encoding_format")]
        public string encoding_format { get; set; }

    }
}
