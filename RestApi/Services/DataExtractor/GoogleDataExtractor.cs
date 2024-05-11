using RestApi.Controllers;
using RestApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;
using System;
using System.IO;
using RestApi.Helpers;

namespace RestApi.Services.DataExtractor
{
    public class GoogleDataExtractor : IDataExtractor
    {
        IConfiguration _config;

        public GoogleDataExtractor(IConfiguration configuration)
        {
            this._config = configuration;
        }

        public RestApi.Models.Document Extract(string data, string docType, ExtractionRequest extractionRequest)
        {
            return ProcessDocument(data);
        }
         
        private RestApi.Models.Document ProcessDocument(string data)
        {
            GoogleEngineConfig engineConfig = CreateGoogleEngineConfig();
            
            //TODO: use file content from request
            string filepath = "D:\\Data\\ADE\\bill.pdf";
            string mimeType = "application/pdf";

            var googleDoc = PerformExtraction(engineConfig, filepath, mimeType);

            GoogleDocumentObjectConverter.ConvertDocument(googleDoc, out RestApi.Models.Document document);

            return document;
        }

        private GoogleEngineConfig CreateGoogleEngineConfig()
        {
            return new GoogleEngineConfig(
                _config["ExtractionConfig:Engine:Config:ProjectId"],
                _config["ExtractionConfig:Engine:Config:LocationId"],
                _config["ExtractionConfig:Engine:Config:ProcessorId"]
                );
        }

        public Google.Cloud.DocumentAI.V1.Document PerformExtraction(
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

}
