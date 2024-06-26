using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Model
{
    public class Value
    {        
        public string ValueType { get; set; }
        public string Text { get; set; }
        public int Confidence { get; set; }
        public Rectangle Bounds { get; set; }
        public List<RawChar> Chars { get; set; }
    }
}
