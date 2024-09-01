using System.Drawing;

namespace PaperBoat.Model.Extracted;

public class Snippet
{
    public int Confidence { get; set; }
    public Rectangle Bounds { get; set; }
    public List<RawChar> Chars { get; set; }
}