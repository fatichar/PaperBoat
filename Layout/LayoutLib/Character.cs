using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LayoutLib
{
    class Character : IRect
    {
        #region public properties
        public char Value { get; }
        #endregion

        #region private properties
        private Rectangle Rect { get; }
        #endregion

        #region IRect Impl
        public Rectangle GetRect() => Rect;
        #endregion
    }
}
