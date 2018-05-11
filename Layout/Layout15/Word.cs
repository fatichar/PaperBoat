using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace Layout15
{
    public class Word : IEnumerable<Character>, IRect
    {
        // public properties
        [PublicAPI]
        public int CharCount => Chars.Length;

        // protected properties
        protected readonly ArraySlice<Character> Chars;
#if DEBUG
        protected readonly string Text = "";
#endif

        #region Constructors
        public Word(ImmutableArray<Character> chars, int offset, int charCount)
            :
            this(chars, offset, charCount, Rectangle.Empty)
        {
            Rect = Geometry.GetUnion(this.Chars);
        }

        public Word(ImmutableArray<Character> chars, int offset, int charCount, Rectangle rect)
        {
            if (offset < 0 || offset >= chars.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Length - 1);
            }
            if (charCount < 1 || charCount > chars.Length - offset)
            {
                throw new ArgumentOutOfRangeException(charCount, nameof(charCount), 1, chars.Length - offset);
            }

            Chars = new ArraySlice<Character>(chars, offset, charCount);

            Rect = rect;

#if DEBUG
            Text = chars.Skip(offset).Take(CharCount).Select(c => c.ToString()).Aggregate((c1, c2) => c1 + c2);
#endif
        }
#endregion

#region public methods
        [PublicAPI]
        public Character this[int index] => Chars[index];
        
#if DEBUG
        [PublicAPI]
        public override string ToString() => Text;
#endif

        [PublicAPI]
        public static bool CanStartWith(char ch) => !char.IsWhiteSpace(ch);

        [PublicAPI]
        public static bool CanEndAt(char ch) => char.IsWhiteSpace(ch);
#endregion
        
#region IEnumerable Impl
        public IEnumerator<Character> GetEnumerator() => Chars.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Chars).GetEnumerator();
#endregion

        // IRect Impl
        public Rectangle Rect { get; }
        public Character EndChar
        {
            get
            {
                var globalIndex = Chars.Offset + CharCount;
                return globalIndex < Chars.SourceArray.Length ? Chars.SourceArray[globalIndex] : null;
            }
        }
    }
}
