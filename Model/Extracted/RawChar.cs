using System.Drawing;

namespace PaperBoat.Model.Extracted;

public class RawChar
{
    public char ch {  get; set; }
    public Rectangle rect { get; set; }
    public int Confidence { get; set; }
}