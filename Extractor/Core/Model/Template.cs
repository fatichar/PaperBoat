using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extractor.Core.Model;

namespace PaperBoat.Extractor.Core.Model
{
    public class Template
    {
        public string DocumentName { get; set; }
        public string Version { get; set; }
        public List<Group> Groups { get; set; }
    }
}