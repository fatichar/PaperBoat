using Microsoft.AspNetCore.Mvc;
using PaperBoat;
using PaperBoat.Extractor.Services;

namespace RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExtractionController(ExtractionService extractionService, IConfiguration configuration) : Controller
{
    private IConfiguration _config { get; } = configuration;

    private ExtractionService ExtractionService { get; } = extractionService;

    [HttpPost]
    public Extract Extract(ExtractionRequest request)
    {
        return ExtractionService.Extract(request);
    }

    [HttpGet]
    public string Index() => "Welcome to PaperBoat!";
}