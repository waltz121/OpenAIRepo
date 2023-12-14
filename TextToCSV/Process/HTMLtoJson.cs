using Newtonsoft.Json;
using OpenAiCore.OpenAiRepository.Model.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextToCSV.Process
{
    internal class HTMLtoJson
    {
        private void AddToJsonFile(string content, string url)
        {
            string jsonFilePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\ContextArticle.json";

            // Read existing json data
            var jsonData = File.ReadAllText(jsonFilePath);
            var jsondf = JsonConvert.DeserializeObject<JsonDataFrame>(jsonData)
                                  ?? new JsonDataFrame();

            // Add new json object to list
            jsondf.root.posts.Add(new Posts
            {
                content = content,
                url = url
            });

            // Write the updated list back to the file
            jsonData = JsonConvert.SerializeObject(jsondf);
            File.WriteAllText(jsonFilePath, jsonData);
        }

        private string GetHtmltxt()
        {
            string htmlTxtFilePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\HTMLtxtToConvert.txt";

            //Code for Removing html tags from a string html
            string HtmlTxtfileContent = File.ReadAllText(htmlTxtFilePath);
            string noHTML = Regex.Replace(HtmlTxtfileContent, @"<[^>]+>|&nbsp;", "").Trim();
            noHTML = noHTML.Replace("\r\n", "");

            return noHTML;
        }

        public void main(string _url)
        {
            string Url = _url;
            string htmlTxt = GetHtmltxt();
            AddToJsonFile(htmlTxt, Url);

            Console.WriteLine(htmlTxt);
            //Console.WriteLine(HtmlTxtfileContent);
        }
    }
}
