using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.OpenAiRepository.DTO.PineCone
{
    public class PineConeUpsertRequestDTO
    {
        [JsonPropertyName("vectors")]
        public List<PineConeVectorsDTO> Vectors { get; set; }
        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }
    }
}
