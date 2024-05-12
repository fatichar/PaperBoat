using PaperBoat.Model;

namespace Extractor.Services.DataExtractor;

public interface IDataExtractor
{
    public Extract Extract(ExtractionRequest extractionRequest);
}