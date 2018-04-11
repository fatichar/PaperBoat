using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    public class TextBlock : IEnumerable<Word>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        public int WordCount { get; }
        public int CharCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<Word> Words { get; }
        private int Offset { get; }
#if DEBUG            
        public string Text { get; }
#endif
        #endregion

        #region Constructors
        public TextBlock(IReadOnlyList<Word> words, int offset, int wordCount)
        {
            Words = words;

            if (offset < 0 || offset >= words.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, words.Count - 1);
            }
            if (wordCount < 1 || offset + wordCount > words.Count)
            {
                throw new ArgumentOutOfRangeException(wordCount, nameof(wordCount), 1, words.Count - offset);
            }

            Offset = offset;

            WordCount = wordCount;
            CharCount = Words.Sum(w => w.CharCount) + WordCount - 1;

            Rect = Geometry.GetUnion(words);
#if DEBUG
            Text = words.Skip(offset).Take(WordCount).Select(w => w.ToString()).Aggregate((w1, w2) => w1 + " " + w2);
#endif
        }
        #endregion

        #region public methods
        public Word this[int index]
        {
            get
            {
                if (index < 0 || index >= WordCount)
                {
                    throw new ArgumentOutOfRangeException(index, nameof(index), 0, WordCount - 1);
                }
                return Words[index];
            }
        }
        #endregion

        #region IEnumerable Impl
        public IEnumerator<Word> GetEnumerator() => Words.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Words.GetEnumerator();

        public static bool CanEndAt(char ch)
        {
            return "\r\n\t".Contains(ch);
        }
        #endregion

#if DEBUG
        public override string ToString() => Text;
#endif
    }
}
