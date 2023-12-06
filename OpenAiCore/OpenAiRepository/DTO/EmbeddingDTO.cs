using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO
{
    public class EmbeddingDTO
    {
        [JsonPropertyName("object")]
        public string Object {  get; set; }

        [JsonPropertyName("embedding")]
        public List<float> Embedding {  get; set; }
        
        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
