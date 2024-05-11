using RestApi.Controllers;

namespace RestApi.Services.DataExtractor
{
    public class DataExtractorFactory
    {
        public IDataExtractor CreateDataExtractor(string type, IConfiguration configuration)
        {
            switch (type) 
            {
                case "Azure" :
                    return new AzureDataExtractor(configuration);
                case "Google":
                    return new GoogleDataExtractor(configuration);
                default:
                    throw new ArgumentException("Data Extractor of the type " +  type + " is not defined");
            }
        }
    }
}
