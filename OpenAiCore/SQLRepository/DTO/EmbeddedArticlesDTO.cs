using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore.SQLRepository.DTO
{
    public class EmbeddedArticlesDTO
    {
        public List<List<float>> Embeddings { get; set; }
        public List<string> texts { get; set; }
        public string splitType { get; set; }
        public string url { get; set; }
        public int PostID { get; set; }
        public string BatchStatus { get; set; }
    }
}
