using System.Diagnostics.CodeAnalysis;
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

    //Create Rectangle
    public static Rectangle CreateRectangle(int top, int left, int bottom, int right)
    {
        return new Rectangle
        {
            Top = top,
            Bottom = bottom,
            Left = left,
            Right = right
        };
    }
    public static Rectangle CreateRectangle(float top, float left, float bottom, float right)
    {
        return new Rectangle
        {
            Top = top,
            Bottom = bottom,
            Left = left,
            Right = right
        };
    }
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class RectangleExtensions
{
    public static readonly Rectangle EmptyRectangle = new Rectangle
    {
        Top = 0,
        Bottom = 0,
        Left = 0,
        Right = 0
    };

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

    //union
    public static Rectangle Union(this Rectangle rect1, Rectangle rect2)
    {
        if (rect1.IsEmpty())
        {
            return rect2;
        }

        if (rect2.IsEmpty())
        {
            return rect1;
        }

        return new Rectangle
        {
            Top = rect1.Top < rect2.Top ? rect1.Top : rect2.Top,
            Bottom = rect1.Bottom > rect2.Bottom ? rect1.Bottom : rect2.Bottom,
            Left = rect1.Left < rect2.Left ? rect1.Left : rect2.Left,
            Right = rect1.Right > rect2.Right ? rect1.Right : rect2.Right
        };
    }
}