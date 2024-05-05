using RestApi.Controllers;

namespace RestApi.Services.DataExtractor
{
    public class DataExtractorFactory
    {
        public IDataExtractor CreateDataExtractor(string type, ExtractionRequest extractionRequest)
        {
            switch (type) 
            {
                case "Azure" :
                    return new AzureDataExtractor(extractionRequest);
                default:
                    throw new ArgumentException("Data Extractor of the type " +  type + " is not defined");
            }
        }
    }
}
