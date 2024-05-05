using System.Drawing;

namespace RestApi.Models;

public record Group(string Name, List<Field> Fields, int Confidence, Rectangle Rect);