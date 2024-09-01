using System.Drawing;

namespace PaperBoat.Model.Extracted;

public class Group(string name)
{
    public string Name { get; } = name;
    public List<Field> Fields { get; } = new List<Field>();
    public Rectangle Rect { get; } = Rectangle.Empty;
}