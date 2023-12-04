using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO
{
    public class ChatCompletionResponseDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("created")]
        public int Created { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("usage")]
        public UsageDTO Usage { get; set; }
        [JsonPropertyName("choices")]
        public ChoicesDTO[] Choices { get; set; }
    }
}
