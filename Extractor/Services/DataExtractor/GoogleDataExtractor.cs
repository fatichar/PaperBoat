using System.Buffers.Text;
using Extractor.Helpers;
using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;
using Microsoft.Extensions.Configuration;
using PaperBoat.Model;

namespace Extractor.Services.DataExtractor;

public class GoogleDataExtractor(IConfiguration configuration) : IDataExtractor
{
    public Extract Extract(ExtractionRequest request)
    {
        return ProcessDocument(request);
    }

    private Extract ProcessDocument(ExtractionRequest request)
    {
        var engineConfig = CreateGoogleEngineConfig();
        var mimeType = GetMimeType(request);

        var googleDoc = PerformExtraction(engineConfig, request.Content, mimeType);

        var document = GoogleMapper.ConvertDocument(googleDoc);
        return document;
    }

    private static string GetMimeType(ExtractionRequest request)
    {
        switch (request.FileType.ToLower())
        {
            case "pdf":
                default:
                return "application/pdf";
            case "png":
            case "jpg":
            case "jpeg":
                return "image/jpeg";
            case "txt":
                return "text/plain";
        }
    }

    private GoogleEngineConfig CreateGoogleEngineConfig()
    {
        return new GoogleEngineConfig(
            configuration["ExtractionConfig:Engine:Config:ProjectId"],
            configuration["ExtractionConfig:Engine:Config:LocationId"],
            configuration["ExtractionConfig:Engine:Config:ProcessorId"]
        );
    }

    private Google.Cloud.DocumentAI.V1.Document PerformExtraction(
        GoogleEngineConfig config,
        string fileData,
        string mimeType
    )
    {
        // Create client
        var client = new DocumentProcessorServiceClientBuilder
        {
            Endpoint = $"{config.LocationId}-documentai.googleapis.com"
        }.Build();

        // Read in local file
        var rawDocument = new RawDocument
        {
            Content = ByteString.FromBase64(fileData),
            MimeType = mimeType
        };

        // Initialize request argument(s)
        var request = new ProcessRequest
        {
            Name = ProcessorName.FromProjectLocationProcessor(config.ProjectId,
                config.LocationId, config.ProcessorId).ToString(),
            RawDocument = rawDocument
        };

        // Make the request
        var response = client.ProcessDocument(request);

        var document = response.Document;
        Console.WriteLine(document.Text);
        return document;
    }
}