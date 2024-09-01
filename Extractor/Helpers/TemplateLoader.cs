using Microsoft.Extensions.Configuration;
using System.Text.Json;
using PaperBoat.Extractor.Core.Model;
using PaperBoat.Model.Extracted;

namespace Extractor.Helpers;

public class TemplateLoader(IConfiguration configuration)
{
    public Template LoadTemplate(string documentName)
    {
        var template = new Template();

        string path = configuration["TemplatePath"] + "/" + documentName + ".json";
        if (path != null)
        {
            string templateText = File.ReadAllText(path);
            template = JsonSerializer.Deserialize<Template>(templateText);
        }
        else
        {
            Console.WriteLine("Error: Template path not configured. Cannot load templates.");
            throw new Exception("Template path not configured. Cannot load templates.");
        }

        return template;
    }
}