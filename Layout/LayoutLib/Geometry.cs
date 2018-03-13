using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    internal class Geometry
    {
        internal static Rectangle GetUnion(IReadOnlyList<IRect> iRects)
        {
            var rects = iRects.Select(r => r.GetRect());

            var mergedRect = rects.Aggregate(Rectangle.Union);

            return mergedRect;
        }
    }
}