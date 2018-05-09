using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Layout15
{
    public class Word<TChar> : IEnumerable<TChar>
    {
        // public properties
        [PublicAPI]
        public int CharCount => Children.Length;

        // protected properties
        protected readonly ArraySlice<TChar> Children;
        protected readonly string Text;

        #region Constructors
        public Word(ImmutableArray<TChar> chars, int offset, int charCount)
        {
            if (offset < 0 || offset >= chars.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, chars.Length - 1);
            }
            if (charCount < 1 || charCount > chars.Length - offset)
            {
                throw new ArgumentOutOfRangeException(charCount, nameof(charCount), 1, chars.Length - offset);
            }

            Children = new ArraySlice<TChar>(chars, offset, charCount);
            
            Text = chars.Skip(offset).Take(CharCount).Select(c => c.ToString()).Aggregate((c1, c2) => c1 + c2);
        }
        #endregion

        #region public methods
        [PublicAPI]
        public TChar this[int index] => Children[index];

        [PublicAPI]
        public override string ToString() => Text;

        [PublicAPI]
        public static bool CanStartWith(char ch) => !char.IsWhiteSpace(ch);

        [PublicAPI]
        public static bool CanEndAt(char ch) => char.IsWhiteSpace(ch);
        #endregion
        
        #region IEnumerable Impl
        public IEnumerator<TChar> GetEnumerator() => Children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Children).GetEnumerator();
        #endregion
    }
}
