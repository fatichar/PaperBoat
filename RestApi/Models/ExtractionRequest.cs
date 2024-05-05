namespace RestApi.Controllers;

public record ExtractionRequest(string FileType, String DocType, string content);