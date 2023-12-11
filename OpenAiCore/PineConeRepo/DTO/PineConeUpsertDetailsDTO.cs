using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeUpsertDetailsDTO
    {
        [JsonPropertyName("typeUrl")]
        public string TypeUrl { get; set; }
        
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
