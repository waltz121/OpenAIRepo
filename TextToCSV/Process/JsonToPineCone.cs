using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.PineConeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToCSV.Process
{
    internal class JsonToPineCone
    {
        PineConeRepository _pineConeRepository;
        OpenAiAPIRepository _openAiRepository;
        public void main()
        {
            _pineConeRepository = new PineConeRepository();
            _openAiRepository = new OpenAiAPIRepository();
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "1000f1c9-9a38-471a-bdc5-483957668b0d");

            
        }
    }
}
