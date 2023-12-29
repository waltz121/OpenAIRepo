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

        public List<ArticlesForProcessingDTO> GetArticlesForProcessing(string sql)
        {
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
