using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeQueryRequestDTO
    {
        [JsonPropertyName("includeValues")]
        public string IncludeValues { get; set; }
        
        [JsonPropertyName("topK")]
        public int TopK { get; set; }
        
        [JsonPropertyName("includeMetadata")]
        public string IncludeMetadata { get; set; }
        
        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }
       
        [JsonPropertyName("vector")]
        public List<float> Vector { get; set; }
    }
}
