
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using RestApi.Services;

namespace RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExtractionController(ExtractionService extractionService) : Controller
{
    private ExtractionService ExtractionService { get; } = extractionService;

    [HttpPost]
    public Document Extract(ExtractionRequest request)
    {
        return ExtractionService.Extract(request);
    }

    [HttpGet]
    public string Index() => "Welcome to PaperBoat!";
}