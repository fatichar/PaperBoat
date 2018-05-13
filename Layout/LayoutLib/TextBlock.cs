using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    public class TextBlock : IEnumerable<Word>, IRect
    {
        // public properties
        public int WordCount => Words.Length;
        public int CharCount { get; }

        // private properties
        protected readonly ArraySlice<Word> Words;
#if DEBUG
        protected readonly string Text = "";
#endif

        #region Constructors
        public TextBlock(ImmutableArray<Word> words, int offset, int wordCount)
            :
            this(words, offset, wordCount, Rectangle.Empty)
        {
            Rect = Geometry.GetUnion(this.Words);
        }

        public TextBlock(ImmutableArray<Word> words, int offset, int wordCount, Rectangle rect)
        {
            if (offset < 0 || offset >= words.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, words.Length - 1);
            }
            if (wordCount < 1 || offset + wordCount > words.Length)
            {
                throw new ArgumentOutOfRangeException(wordCount, nameof(wordCount), 1, words.Length - offset);
            }

            Words = new ArraySlice<Word>(words, offset, wordCount);

            Rect = rect;

            CharCount = Words.Sum(w => w.CharCount) + WordCount - 1;

#if DEBUG
            Text = words.Skip(offset).Take(WordCount).Select(w => w.ToString()).Aggregate((w1, w2) => w1 + " " + w2);
#endif
        }
#endregion

#region public methods
        public Word this[int index] => Words[index];

        public static bool CanEndAt(char ch)
        {
            return "\r\n\t".Contains(ch);
        }
        
#if DEBUG
        public override string ToString() => Text;
#endif
#endregion
        
#region IEnumerable Impl
        public IEnumerator<Word> GetEnumerator() => Words.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Words).GetEnumerator();
#endregion
        
        // IRect Impl
        public Rectangle Rect { get; }
        public Character EndChar => Words.Last().EndChar;
    }
}
