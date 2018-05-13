using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    internal static class Geometry
    {
        internal static Rectangle GetUnion(IEnumerable<IRect> iRects)
        {
            var rects = iRects.Select(r => r.Rect);

            var mergedRect = rects.Aggregate(Rectangle.Union);

            return mergedRect;
        }
    }
}