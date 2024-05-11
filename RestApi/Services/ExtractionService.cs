using RestApi.Controllers;
using RestApi.Models;
using RestApi.Services.DataExtractor;
using System.Configuration;

namespace RestApi.Services
{ 
    public class ExtractionService
    {
        public ExtractionService(IConfiguration config)
        {
            _config = config;
        }

        private IConfiguration _config { get; }

        public Document Extract(ExtractionRequest request)
        {
            var response = new Document("Abc", new List<Group>());

            DataExtractorFactory dataExtractorFactory = new DataExtractorFactory();
            IDataExtractor dataExtractor = dataExtractorFactory.CreateDataExtractor(_config["ExtractionConfig:Engine:Provider"], _config);

            return dataExtractor.Extract(request.content, request.DocType, request);
        }
    }
}