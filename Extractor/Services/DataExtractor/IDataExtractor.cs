namespace PaperBoat.Extractor.Services.DataExtractor;

public interface IDataExtractor
{
    public Extract Extract(ExtractionRequest extractionRequest);
}