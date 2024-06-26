using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Model
{
    public class RawChar
    {
        public char ch {  get; set; }
        public Rectangle rect { get; set; }
        public int Confidence { get; set; }
    }
}
