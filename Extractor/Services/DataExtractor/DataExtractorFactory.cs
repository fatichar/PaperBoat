using Microsoft.Extensions.Configuration;

namespace Extractor.Services.DataExtractor;

public class DataExtractorFactory
{
    public const string AZURE = "Azure";
    public const string GOOGLE = "Google";


    public static IDataExtractor CreateDataExtractor(string provider, IConfiguration configuration)
    {
        switch (provider)
        {
            case AZURE :
                return new AzureDataExtractor(configuration);
            case GOOGLE:
                return new GoogleDataExtractor(configuration);
            default:
                throw new ArgumentException("Data Extractor of the type " +  provider + " is not defined");
        }
    }
}