using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Layout15
{
    public class TextLine<TBlock, TWord, TChar> : IEnumerable<TBlock> where TBlock : TextBlock<TWord, TChar> where TWord : Word<TChar>
    {
        // public properties
        [PublicAPI]
        public int TextBlockCount { get; }
        [PublicAPI]
        public int WordCount { get; }
        [PublicAPI]
        public int CharCount { get; }

        #region protected properties

        protected readonly ArraySlice<TBlock> Children;
#if DEBUG
        private string Text { get; }
#endif
        #endregion

        #region Constructors
        public TextLine(ImmutableArray<TBlock> textBlocks, int offset, int textBlockCount)
        {
            Children = new ArraySlice<TBlock>(textBlocks, offset, textBlockCount);

            if (offset < 0 || offset >= textBlocks.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textBlocks.Length - 1);
            }
            if (textBlockCount < 1 || offset + textBlockCount > textBlocks.Length)
            {
                throw new ArgumentOutOfRangeException(textBlockCount, nameof(textBlockCount), 1, textBlocks.Length - offset);
            }
            
            WordCount = Children.Sum(b => b.WordCount);
            CharCount = Children.Sum(b => b.CharCount) + TextBlockCount - 1;

            Text = textBlocks.Skip(offset).Take(TextBlockCount).Select(tb => tb.ToString()).Aggregate((tb1, tb2) => tb1 + "    " + tb2);
        }
        #endregion

        #region public methods
        public TBlock this[int index] => Children[index];

        [PublicAPI]
        public static bool CanEndAt(char ch)
        {
            return ch == '\n';
        }

        public override string ToString() => Text;
        #endregion
        
        #region IEnumerable Impl
        public IEnumerator<TBlock> GetEnumerator() => Children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Children).GetEnumerator();
        #endregion
    }
}
