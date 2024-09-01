using System.Drawing;

namespace PaperBoat.Model.Extracted;

public class Value
{
    public string ValueType { get; set; }
    public string Text { get; set; }
    public int Confidence { get; set; }
    public Rectangle Bounds { get; set; }
    public List<RawChar> Chars { get; set; }
}