using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore.OpenAiRepository.Model
{
    public class EmbeddingRanking
    {
        public string Percent { get; set; }
        public float Relatedness { get; set; }
        public string Text {  get; set; }
    }
}
