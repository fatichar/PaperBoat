using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

using JetBrains.Annotations;

namespace LayoutLib
{
    public class Word : IEnumerable<Character>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        [PublicAPI]
        public int CharCount { get; }
        #endregion

        #region private properties
        private ImmutableList<Character> Characters { get; }
        private int Offset { get; }
#if DEBUG
        private string Text { get; }
#endif
        #endregion

        #region Constructors
        public Word(ImmutableList<Character> chars, int offset, int charCount)
        {
            if (offset < 0 || offset >= chars.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Count - 1);
            }
            if (charCount < 1 || charCount > chars.Count - offset)
            {
                throw new ArgumentOutOfRangeException(charCount, nameof(charCount), 1, chars.Count - offset);
            }

            Characters = chars;

            Offset = offset;
            CharCount = charCount;
            
            Rect = Geometry.GetUnion(chars);
#if DEBUG
            Text = chars.Skip(offset).Take(CharCount).Select(c => c.ToString()).Aggregate((c1, c2) => c1 + c2);
#endif
        }
        #endregion

        #region public methods
        [PublicAPI]
        public Character this[int index]
        {
            get
            {
                if (index < 0 || index >= CharCount)
                {
                    throw new ArgumentOutOfRangeException(index, nameof(index), 0, CharCount - 1);
                }
                return Characters[Offset + index];
            }
        }
        #endregion

        #region IEnumerable Impl
        public IEnumerator<Character> GetEnumerator() => Characters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Characters.GetRange(Offset, CharCount).GetEnumerator();
        #endregion

        [PublicAPI]
        public static bool CanStartWith(char ch) => !char.IsWhiteSpace(ch);

        [PublicAPI]
        public static bool CanEndAt(char ch) => char.IsWhiteSpace(ch);

#if DEBUG            
        public override string ToString() => Text;
#endif
    }
}
