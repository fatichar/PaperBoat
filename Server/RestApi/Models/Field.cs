using System.Drawing;

namespace RestApi.Models;

public record Field(string Name, string ValueType)
{
    public string? Value { get; init; }
    public int Confidence { get; init; }
    public Rectangle Rect { get; init; }
    public List<RawChar> RawText { get; init; } = new();
}