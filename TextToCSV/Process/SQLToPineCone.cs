using OpenAiCore.Shared;
using OpenAiCore.SQLRepository;
using OpenAiCore.SQLRepository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToCSV.Process
{
    public class SQLToPineCone
    {
        SQLDataRepository sqlRepository;
        StringCodeUtility codeUtility;
        public SQLToPineCone() { 
            codeUtility = new StringCodeUtility();
            sqlRepository = new SQLDataRepository();
        }
        public async Task Main()
        {            
            List<ArticlesForProcessingDTO> articlesForProcessingDTOs = sqlRepository.GetArticlesForProcessing();

            foreach(var article in articlesForProcessingDTOs)
            {
                var htmlInput = article.FormattedBody;

                htmlInput = codeUtility.RemoveScriptsStylesAndHtml(htmlInput);
            }

        }
    }
}
