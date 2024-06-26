using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Model
{
    public class Field
    {
        public string Name { get; set; }
        public string Id { get; set; }        
        public string ValueType { get; set; }        

        public Value Value { get; set; }        
        public Snippet Snippet { get; set; }

        public ExtractionAlgo ExtractionAlgo { get; set; }
    }
}
