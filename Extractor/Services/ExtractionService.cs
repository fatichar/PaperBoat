using Extractor.Services.DataExtractor;
using Microsoft.Extensions.Configuration;
using PaperBoat.Model;

namespace Extractor.Services;

public class ExtractionService
{
    private const string DEFAULT_PROVIDER = DataExtractorFactory.AZURE;

    private IConfiguration Config { get; }
    private readonly IDataExtractor _extractor;

    public ExtractionService(IConfiguration config)
    {
        Config = config;

        _extractor = DataExtractorFactory.CreateDataExtractor(
            Config["ExtractionConfig:Engine:Provider"] ?? DEFAULT_PROVIDER,
            Config);
    }

    public Extract Extract(ExtractionRequest request)
    {
        return _extractor.Extract(request);
    }
}