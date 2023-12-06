// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;

string filePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\OpenAiDataset.txt";
string csvFilePath = @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\TextToCSV\Files\OpenAiDataset.csv";
string fileContent;
List<string> csvContent = new List<string>();

using (StreamReader reader = new StreamReader(filePath))
{
    fileContent = reader.ReadToEnd();
}

int Charlimit = 120;
int CharCounter = 0;
string tmpStr = "";
foreach (var character in fileContent)
{
    tmpStr = tmpStr + character;
    if(CharCounter >= Charlimit)
    {
        if(character == '.' || character == '?' || character == '!')
        {
            csvContent.Add(tmpStr);
            tmpStr = "";
            CharCounter = 0;
        }
    }
    CharCounter++;
}

var csv = new StringBuilder();
csv.AppendLine("ID,TEXT");
int idCtr = 1;
foreach(var i in csvContent)
{
    var formattedStr = i.Replace(",", "");
    csv.AppendLine(idCtr + "," + formattedStr);
    Console.WriteLine(csv);
    Console.WriteLine(" ============================================= ");

    idCtr++;
}
File.WriteAllText(csvFilePath, csv.ToString());
Console.WriteLine("Finished converting to CSV");
Console.ReadLine();