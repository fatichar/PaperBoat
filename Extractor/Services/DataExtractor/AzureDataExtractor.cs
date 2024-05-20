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

        var docContent = new AnalyzeDocumentContent()
        {
            Base64Source = BinaryData.FromBytes(Convert.FromBase64String(content))
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

        var document = AzureMapper.ConvertDocument(new InClassName(analyzedDocument, docType));

        return document;
    }
}