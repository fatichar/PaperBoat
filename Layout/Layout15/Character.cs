using System.Drawing;

namespace Layout15
{
    public class Character : IRect
    {
        // IRect Impl
        public Rectangle Rect { get; }

        // public properties
        public char Value => _chars[_offset];

        // private properties
        private readonly string _chars;
        private readonly int _offset;

        #region Constructors
        public Character(string chars, int offset, Rectangle rect)
        {
            if (offset < 0 || offset >= chars.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Length - 1);
            }

            _chars  = chars;
            _offset = offset;
            Rect    = rect;
        }
        #endregion
        
        public override string ToString() => Value.ToString();
    }
}
