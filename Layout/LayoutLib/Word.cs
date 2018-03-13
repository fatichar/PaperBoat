using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LayoutLib
{
    class Word : IEnumerable<Character>, IRect
    {
        #region public properties
        public int CharCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<Character> Characters { get; }
        private int Offset { get; }
        private Rectangle Rect { get; }
        #endregion

        #region Constructors
        public Word(IReadOnlyList<Character> chars, int offset, int charCount)
        {
            Characters = chars;

            if (offset < 0 || offset >= chars.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Count - 1);
            }
            if (charCount < 1 || offset + charCount >= chars.Count)
            {
                throw new ArgumentOutOfRangeException(charCount, nameof(charCount), 1, chars.Count - offset);
            }

            Offset = offset;
            CharCount = charCount;
            
            Rect = Geometry.GetUnion(chars);
        }
        #endregion

        #region public methods
        public Character this[int index]
        {
            get
            {
                if (index < 0 || index >= CharCount)
                {
                    throw new ArgumentOutOfRangeException(index, nameof(index), 0, CharCount - 1);
                }
                return Characters[index];
            }
        }
        #endregion

        #region IEnumerable Impl
        public IEnumerator<Character> GetEnumerator() => Characters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Characters.GetEnumerator();
        #endregion

        #region IRect Impl
        public Rectangle GetRect() => Rect;
        #endregion
    }
}
