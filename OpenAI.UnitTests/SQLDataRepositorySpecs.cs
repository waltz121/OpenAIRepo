using OpenAiCore;
using OpenAiCore.Shared;
using OpenAiCore.SQLRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class SQLDataRepositorySpecs
    {
        SQLDataRepository sqlDataRepository;
        StringCodeUtility codeUtility;
        public SQLDataRepositorySpecs() {
            Config.Init("","","", "Data Source=PRODDBSOCIAL.NEWMERCOLA.COM;Initial Catalog=CommunityServer;User ID=webuser_communityserver;Password=w3bu$3r_c0mmun1ty$3rv3r!;Encrypt=False");
            sqlDataRepository = new SQLDataRepository();
        }
        [TestMethod]
        public void GetArticlesForProcessing_InSQL()
        {
            var ArticlesForProcessing = sqlDataRepository.GetArticlesForProcessing();
        }

        [TestMethod]
        public void SQLtoPineConeDataPipeline()
        {

        }
    }
}
