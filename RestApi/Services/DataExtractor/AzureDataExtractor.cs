using RestApi.Controllers;
using RestApi.Models;
using System.Drawing;

using Azure;
using Azure.AI.DocumentIntelligence;
using System.Security.Cryptography;
using System.Buffers.Text;
using RestApi.Helpers;

namespace RestApi.Services.DataExtractor
{
    public class AzureDataExtractor : IDataExtractor
    {
        private string endpoint = "https://testextraction2.cognitiveservices.azure.com/";
        private string key = "258957c923c74144a08715a9063bdba2";
        private ExtractionRequest _extractionRequest;
        IConfiguration _config ;

        public AzureDataExtractor(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Document Extract(string content, string docType, ExtractionRequest extractionRequest)
        {
            _extractionRequest = extractionRequest;
            return ProcessDocument(content).Result;            
        }

        private async Task<Document> ProcessDocument(string content)
        {
            AzureKeyCredential credential = new AzureKeyCredential(key);
            DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
            string docType = "prebuilt-invoice";

            //sample invoice document

            byte[] imgData = File.ReadAllBytes("D:\\Data\\ADE\\bill.jpeg");

            System.BinaryData binaryData = BinaryData.FromBytes(imgData);

            //Uri invoiceUri = new Uri("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-invoice.pdf");
            //Uri invoiceUri = new Uri("D:\\Data\\ADE\\bill.jpeg");

            AnalyzeDocumentContent docContent = new AnalyzeDocumentContent()
            {
                Base64Source = binaryData
            };

            Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, docType, docContent);

            AnalyzeResult result = operation.Value;

            for (int i = 0; i < result.Documents.Count; i++)
            {
                Console.WriteLine($"Document {i}:");

                AnalyzedDocument analyzedDocument = result.Documents[i];
                
                AzureDocumentObjectConverter.ConvertDocument(analyzedDocument,
                    docType,
                    out Models.Document document);
                return document;
            }
            return null;
        }        
    }
}
