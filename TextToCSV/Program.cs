// See https://aka.ms/new-console-template for more information

using TextToCSV;
using TextToCSV.Process;

// =================  Text to Json =================
//Console.WriteLine("Converting Html Text to Json ... ");
//HTMLtoJson hTMLtoJson = new HTMLtoJson();
//hTMLtoJson.main("");
//Console.WriteLine("Finished Converting HTML Text to Json ... ");

// =================  Json to Embedded Json =================
//Console.WriteLine("Converting Json to Embedded Json ... "); 
//JsonToEmbeddedJson jsonToEmbeddedJson = new JsonToEmbeddedJson(); 
//await jsonToEmbeddedJson.main();
//Console.WriteLine("Finished Converting Json to Embedded Json ... ");

// =================  Embedded Json to PineCone =================

//Console.WriteLine("Saving Embedded Json to PineCone ... ");
//JsonToPineCone jsonToPineCone = new JsonToPineCone();
//await jsonToPineCone.main();
//Console.WriteLine("Finished Saving Embedded Json to PineCone ... ");


// =================  SQL to PineCone =================
Console.WriteLine("Saving SQL to PineCone ... ");
SQLToPineCone sQLToPineCone = new SQLToPineCone();
await sQLToPineCone.Main();
Console.WriteLine("Finished Saving SQL to PineCone ... ");