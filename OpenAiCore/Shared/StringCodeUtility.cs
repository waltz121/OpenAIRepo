using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenAiCore.Shared
{
    public class StringCodeUtility
    {
        public string GetHtmlInput(string url)
        {
            string htmlInput = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    htmlInput = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                htmlInput = "Error: " + ex.Message;
            }

            return htmlInput;
        }
        public string RemoveScriptsStylesAndHtml(string htmlInput)
        {
            // Remove javascripts from htmlInput
            htmlInput = Regex.Replace(htmlInput, "<script.*?</script>", string.Empty, RegexOptions.Singleline);

            // Remove CSS from htmlInput
            htmlInput = Regex.Replace(htmlInput, "<style.*?</style>", string.Empty, RegexOptions.Singleline);

            // Remove html tags from htmlInput
            htmlInput = Regex.Replace(htmlInput, "<.*?>", string.Empty, RegexOptions.Singleline);

            // Remove whitespaces from htmlInput
            htmlInput = Regex.Replace(htmlInput, @"\s+", " ");

            return htmlInput;
        }
        public List<string> SplitTextToLimit(int CharLimit, string Text)
        {
            int CharCounter = 0;
            string tempText = "";
            List<string> TextList = new List<string>();
            foreach (var character in Text)
            {
                tempText = tempText + character;
                if (CharCounter >= CharLimit)
                {
                    if (character == '.' || character == '?' || character == '!')
                    {
                        // Add to list
                        TextList.Add(tempText);
                        tempText = "";
                        CharCounter = 0;
                    }
                }
                CharCounter++;
            }
            return TextList;
        }
    }
}