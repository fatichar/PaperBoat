using Azure;
using Azure.AI.DocumentIntelligence;
using Extractor.Helpers;
using Microsoft.Extensions.Configuration;
using PaperBoat.Model;

namespace Extractor.Services.DataExtractor;

public class AzureDataExtractor(IConfiguration configuration) : IDataExtractor
{
    IConfiguration _config = configuration;
    private const string ENDPOINT = "https://testextraction2.cognitiveservices.azure.com/";
    private const string KEY = "258957c923c74144a08715a9063bdba2";

    public Extract Extract(ExtractionRequest request)
    {
        return ProcessDocument(request.Content).Result;
    }

    private async Task<Extract> ProcessDocument(string content)
    {
        var credential = new AzureKeyCredential(KEY);
        var client = new DocumentIntelligenceClient(new Uri(ENDPOINT), credential);
        var docType = "prebuilt-invoice";

        //sample invoice document

        var imgData = File.ReadAllBytes("D:\\Data\\ADE\\bill.jpeg");

        var binaryData = BinaryData.FromBytes(imgData);

        //Uri invoiceUri = new Uri("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-invoice.pdf");
        //Uri invoiceUri = new Uri("D:\\Data\\ADE\\bill.jpeg");

        var docContent = new AnalyzeDocumentContent()
        {
            Base64Source = binaryData
        };

        Operation<AnalyzeResult> operation =
            await client.AnalyzeDocumentAsync(WaitUntil.Completed, docType, docContent);

        var result = operation.Value;
        if (result.Documents.Count == 0)
        {
            Console.WriteLine("No documents were extracted.");
            return new Extract();
        }

        var analyzedDocument = result.Documents[0];

        AzureDocumentObjectConverter
            .ConvertDocument(analyzedDocument, docType, out var document);

        return document;
    }
}