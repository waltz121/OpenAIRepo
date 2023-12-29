using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenAiCore.SQLRepository.DTO
{
    public class ArticlesForProcessingDTO
    {
        [JsonPropertyName("PostID")]
        public int PostID { get; set; }

        [JsonPropertyName("PostUrl")]
        public string PostUrl { get; set; }
        
        [JsonPropertyName("FormattedBody")]
        public string FormattedBody { get; set; }
        
        [JsonPropertyName("SubstackURL")]
        public string SubstackURL { get; set; }        
    }
}
