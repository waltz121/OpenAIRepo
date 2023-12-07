// See https://aka.ms/new-console-template for more information

using TextToCSV;
using TextToCSV.Process;

TextToCSVBasic textToCSVBasic = new TextToCSVBasic();
textToCSVBasic.main();

Console.WriteLine("Finished converting to CSV");
Console.ReadLine();