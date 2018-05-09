using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Layout15.Strong;

namespace Layout15
{
    public class TextBlock<TWord, TChar> : IEnumerable<TWord> where TWord : Word<TChar>
    {
        // public properties
        public int WordCount { get; }
        public int CharCount { get; }

        // private properties
        protected readonly ArraySlice<TWord> Children;
        protected readonly string Text;

        #region Constructors
        public TextBlock(ImmutableArray<TWord> words, int offset, int wordCount)
        {
            if (offset < 0 || offset >= words.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, words.Length - 1);
            }
            if (wordCount < 1 || offset + wordCount > words.Length)
            {
                throw new ArgumentOutOfRangeException(wordCount, nameof(wordCount), 1, words.Length - offset);
            }

            Children = new ArraySlice<TWord>(words, offset, wordCount);
            
            WordCount = wordCount;
            CharCount = Children.Sum(w => w.CharCount) + WordCount - 1;
            
            Text = words.Skip(offset).Take(WordCount).Select(w => w.ToString()).Aggregate((w1, w2) => w1 + " " + w2);
        }
        #endregion

        #region public methods
        public TWord this[int index] => Children[index];

        public static bool CanEndAt(char ch)
        {
            return "\r\n\t".Contains(ch);
        }
        
        public override string ToString() => Text;
        #endregion
        
        #region IEnumerable Impl
        public IEnumerator<TWord> GetEnumerator() => Children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Children).GetEnumerator();
        #endregion
    }
}
