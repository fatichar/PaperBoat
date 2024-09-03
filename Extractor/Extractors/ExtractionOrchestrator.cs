using Google.Cloud.DocumentAI.V1;
using Microsoft.Extensions.Configuration;
using PaperBoat.Extractor.Helpers;

namespace PaperBoat.Extractor.Extractors;

internal class ExtractionOrchestrator(Document document, IConfiguration configuration, string docType)
{
    TemplateLoader _templateLoader = new TemplateLoader(configuration);

    public Template ExtractData()
    {
        var template = _templateLoader.LoadTemplate(docType);

        FindGroups(document, template);

        foreach (var group in template.Groups)
        {
            ExtractGroup(group);
        }

        return template;
    }

    private static void FindGroups(Document document, Template template)
    {
        //TODO: Algo for group detection
        template.Groups[0].Rect = new System.Drawing.Rectangle(0, 0,
            (int)document.Pages[0].Dimension.Width, (int)document.Pages[0].Dimension.Height);
    }

    private void ExtractGroup(Group group)
    {
        foreach (var field in group.Fields)
        {
            ExtractField(field, group.Rect);
        }
    }

    private void ExtractField(Field field, System.Drawing.Rectangle rect)
    {

    }

}