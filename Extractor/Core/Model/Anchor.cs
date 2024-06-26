using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Core.Model
{
    public enum AnchorType
    {
        FIXED,
        VALUE_TYPE
    }

    public enum AnchorPosition
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    public class Anchor
    {
        public AnchorType Type {  get; set; }
        public AnchorPosition Position { get; set; }
        public string Value { get; set; }
    }
}
