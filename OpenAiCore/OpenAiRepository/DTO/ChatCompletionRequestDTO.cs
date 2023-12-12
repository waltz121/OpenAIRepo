using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.OpenAi
{
    public class ChatCompletionRequestDTO
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "gpt-3.5-turbo";
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 4000;
        [JsonPropertyName("messages")]
        public List<MessagesDTO> Messages { get; set; }
        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false;
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.5;
    }
}
