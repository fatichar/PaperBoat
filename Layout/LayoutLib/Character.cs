using System.Collections.Generic;
using System.Drawing;

namespace LayoutLib
{
    public class Character : IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        public char Value => Chars[Offset];
        #endregion

        #region private properties
        private IReadOnlyList<char> Chars { get; }
        private int Offset { get; }
        #endregion

        #region Constructors
        public Character(IReadOnlyList<char> chars, int offset, Rectangle rect)
        {
            if (offset < 0 || offset >= chars.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Count - 1);
            }

            Chars = chars;
            Offset = offset;
            Rect = rect;
        }
        #endregion
    }
}
