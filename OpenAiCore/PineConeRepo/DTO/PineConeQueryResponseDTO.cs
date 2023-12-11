using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.PineConeRepository.DTO
{
    public class PineConeQueryResponseDTO
    {
        [JsonPropertyName("matches")]
        public List<PineConeQueryMatchesDTO> Matches { get; set; }
        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }
    }
}
