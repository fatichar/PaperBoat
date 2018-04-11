using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    public class Word : IEnumerable<Character>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        public int CharCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<Character> Characters { get; }
        private int Offset { get; }
#if DEBUG            
        public string Text { get; }
#endif
        #endregion

        #region Constructors
        public Word(IReadOnlyList<Character> chars, int offset, int charCount)
        {
            if (offset < 0 || offset >= chars.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Count - 1);
            }
            if (charCount < 1 || offset + charCount > chars.Count)
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

        public static bool CanStartWith(char ch) => !char.IsWhiteSpace(ch);

        public static bool CanEndAt(char ch) => char.IsWhiteSpace(ch);

#if DEBUG            
        public override string ToString() => Text;
#endif
    }
}
