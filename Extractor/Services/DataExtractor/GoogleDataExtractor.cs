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
        return ProcessDocument(request.Content);
    }

    private Extract ProcessDocument(string data)
    {
        var engineConfig = CreateGoogleEngineConfig();

        //TODO: use file content from request
        var filepath = "D:\\Data\\ADE\\bill.pdf";
        var mimeType = "application/pdf";

        var googleDoc = PerformExtraction(engineConfig, filepath, mimeType);

        GoogleDocumentObjectConverter.ConvertDocument(googleDoc, out var document);

        return document;
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
        string filepath,
        string mimeType
    )
    {
        // Create client
        var client = new DocumentProcessorServiceClientBuilder
        {
            Endpoint = $"{config.LocationId}-documentai.googleapis.com"
        }.Build();

        // Read in local file
        using var fileStream = File.OpenRead(filepath);
        var rawDocument = new RawDocument
        {
            Content = ByteString.FromStream(fileStream),
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