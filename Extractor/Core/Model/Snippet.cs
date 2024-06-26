using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Model
{
    public class Snippet
    {
        public int Confidence { get; set; }
        public Rectangle Bounds { get; set; }
        public List<RawChar> Chars { get; set; }
    }
}
