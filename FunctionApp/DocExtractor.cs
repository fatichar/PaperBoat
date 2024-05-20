using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaperBoat.Model;

namespace PaperBoat.FunctionApp;

public class DocExtractor(ILogger<DocExtractor> logger)
{
    [Function("extract")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        // Deserialize the request body into the model class
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<ExtractionRequest>(requestBody);

        // Validate the parameters
        if (request == null)
        {
            return new BadRequestObjectResult("No request body was provided.");
        }

        return new OkObjectResult("Extracting data from the " + request.DocType);
    }
}