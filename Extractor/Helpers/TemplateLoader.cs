using Extractor.Core.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Extractor.Helpers
{
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
}
