// See https://aka.ms/new-console-template for more information

using TextToCSV;
using TextToCSV.Process;

//TextToCSVBasic textToCSVBasic = new TextToCSVBasic();
//HTMLtoJson hTMLtoJson = new HTMLtoJson();
//hTMLtoJson.main();

JsonToEmbeddedJson jsonToEmbeddedJson = new JsonToEmbeddedJson(); 
await jsonToEmbeddedJson.main();