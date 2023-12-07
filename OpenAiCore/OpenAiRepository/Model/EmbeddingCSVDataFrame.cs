using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore.OpenAiRepository.Model
{
    public class EmbeddingCSVDataFrame
    {
        public string url {  get; set; }
        public string text { get; set; }
        public List<float> embedding { get; set; }
    }
}
