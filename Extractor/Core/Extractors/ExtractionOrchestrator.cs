using Extractor.Core.Model;
using Extractor.Helpers;
using Google.Cloud.DocumentAI.V1;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Extractors
{
    internal class ExtractionOrchestrator(Document document, IConfiguration configuration, string DocType)
    {
        TemplateLoader templateLoader = new TemplateLoader(configuration);
        
        public Template ExtractData()
        {
            var template = templateLoader.LoadTemplate(DocType);

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
            template.Groups[0].rect = new System.Drawing.Rectangle(0, 0,
                    (int)document.Pages[0].Dimension.Width, (int)document.Pages[0].Dimension.Height);
        }

        private void ExtractGroup(Group group)
        {
            foreach (var field in group.Fields)
            {
                ExtractField(field, group.rect);
            }
        }

        private void ExtractField(Field field, System.Drawing.Rectangle rect)
        {
            
        }

    }
}
