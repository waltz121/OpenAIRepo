using OpenAiCore.SQLRepository.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace OpenAiCore.SQLRepository
{
    public class SQLDataRepository
    {

        public SQLDataRepository()
        {
        }

        public List<ArticlesForProcessingDTO> GetArticlesForProcessing()
        {
            string sql = "Select \r\n  CP.PostID,\r\n  dbo.mercola_GetFullPostsURL_FromPostID(CP.PostID) AS PostUrl,\r\n  CP.FormattedBody,\r\n  ISNULL(AD.SubstackUrl,'') as SubstackUrl                                 \r\n  FROM CS_Posts as CP with (nolock)                                          \r\n  inner join Mercola_NewsletterDetails as AD with (nolock ) on CP.PostID = AD.PostID                                          \r\n  Inner Join Mercola_MapNewsletterPosts as MM with (nolock) on MM.PostID = AD.PostId                                          \r\n  inner join Mercola_PostStatus PS with (nolock) on PS.Postid=CP.PostId \r\n  WHERE   \r\n  AD.NewsletterDate BETWEEN'2023-01-01' AND '2023-02-01'                                  \r\n  AND CP.isapproved =1                                          \r\n  AND PS.StatusID = 7                                          \r\n  AND CP.SectionID=95                                          \r\n  AND AD.IncludeInArticles=1  \r\n  AND (MM.Newslettersortorder = 1 OR MM.Newslettersortorder = 2 OR MM.Newslettersortorder = 3)\r\n  AND PS.IsRedirect = 0\r\nORDER BY AD.NewsletterDate DESC,MM.NewsletterSortOrder";
            List<ArticlesForProcessingDTO> articlesForProcessingDTOs = new List<ArticlesForProcessingDTO>();
            using (var connection = new SqlConnection(Config.SQLConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArticlesForProcessingDTO articlesForProcessingDTO = new ArticlesForProcessingDTO();
                            articlesForProcessingDTO.PostID = reader.GetInt32(0);
                            articlesForProcessingDTO.PostUrl = reader.GetString(1);
                            articlesForProcessingDTO.FormattedBody = reader.GetString(2);
                            articlesForProcessingDTO.SubstackURL = reader.GetString(3);
                            articlesForProcessingDTOs.Add(articlesForProcessingDTO);
                        }
                    }
                }
                connection.Close();
            }
            return articlesForProcessingDTOs;
        }
    }
}
