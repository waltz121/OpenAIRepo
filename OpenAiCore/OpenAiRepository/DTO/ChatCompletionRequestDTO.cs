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
        public double Temperature { get; set; } = 1;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1;

        [JsonPropertyName("seed")]
        public int? Seed { get; set; } = null;

        [JsonPropertyName("frequency_penalty")]
        public double FrequencyPenalty { get; set; } = 0;

        [JsonPropertyName("presence_penalty")]
        public double PresencePenalty { get; set; } = 0;

        [JsonPropertyName("response_format")]
        public ResponseTypeDTO ResponseFormat { get; set; } = null;
    }
}
