using RestApi.Controllers;
using RestApi.Models;

namespace RestApi.Services;

public class ExtractionService
{
    public Document Extract(ExtractionRequest request)
    {
        var response = new Document("Abc", new List<Group>());
        return response;
    }
}