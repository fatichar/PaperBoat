using PaperBoat.Model;

namespace Extractor.Helpers;

public static class ProtoExtensions
{
    public static Extract CreateExtract(string docType, List<Group> groups)
    {
        var extract = new Extract();
        extract.Groups.AddRange(groups);
        return extract;
    }

    public static Group CreateGroup(string name, IEnumerable<Field> fields, int confidence, Rectangle rectangle)
    {
        var group = new Group
        {
            Name = name,
            Confidence = confidence,
            Rect = rectangle
        };
        group.Fields.AddRange(fields);
        return group;
    }

    // extention property Rectangle.IsEmpty

}

public static class RectangleExtensions
{
    public static bool IsEmpty(this Rectangle rect)
    {
        return rect.Width <= 0 || rect.Height <= 0;
    }
}