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
        private string Chars { get; }
        private int Offset { get; }
        #endregion

        #region Constructors
        public Character(string chars, int offset, Rectangle rect)
        {
            if (offset < 0 || offset >= chars.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Length - 1);
            }

            Chars  = chars;
            Offset = offset;
            Rect   = rect;
        }
        #endregion

#if DEBUG
        public override string ToString() => Value.ToString();
#endif
    }
}
