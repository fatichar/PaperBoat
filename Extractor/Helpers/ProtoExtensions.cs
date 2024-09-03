using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace PaperBoat.Extractor.Helpers;

public static class ProtoExtensions
{
    public static Extract CreateExtract(string docType, List<Group> groups)
    {
        var extract = new Extract(groups, 0);
        return extract;
    }

    public static Group CreateGroup(string name, IReadOnlyList<Field> fields, byte confidence)
    {
        return new Group(name, fields, confidence);
    }
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class RectangleExtensions
{
    public static bool IsEmpty(this Rectangle rect)
    {
        return rect.Width() <= 0 || rect.Height() <= 0;
    }

    public static float Width(this Rectangle rect)
    {
        return rect.Right - rect.Left;
    }

    public static float Height(this Rectangle rect)
    {
        return rect.Bottom - rect.Top;
    }
}