using Extractor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaperBoat.Model;

namespace PaperBoat.FunctionApp;

public class DocExtractor(ILogger<DocExtractor> logger, ExtractionService extractionService)
{
    [Function("extract")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        // Deserialize the request body into the model class
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var request = ExtractionRequest.Parser.ParseJson(requestBody);

        // Validate the parameters
        if (request == null)
        {
            return new BadRequestObjectResult("No request body was provided.");
        }

        var extract = extractionService.Extract(request);

        return new OkObjectResult(extract);
    }
}