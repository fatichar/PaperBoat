using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LayoutLib
{
    class TextBlock : IEnumerable<Word>, IRect
    {
        #region public properties
        public int WordCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<Word> Words { get; }
        private int Offset { get; }
        private Rectangle Rect { get; }
        #endregion

        #region Constructors
        public TextBlock(IReadOnlyList<Word> words, int offset, int wordCount)
        {
            this.Words = words;

            if (offset < 0 || offset >= words.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, words.Count - 1);
            }
            if (wordCount < 1 || offset + wordCount >= words.Count)
            {
                throw new ArgumentOutOfRangeException(wordCount, nameof(wordCount), 1, words.Count - offset);
            }

            Offset = offset;
            WordCount = wordCount;

            Rect = Geometry.GetUnion(words);
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
        #endregion

        #region IRect Impl
        public Rectangle GetRect() => Rect;
        #endregion
    }
}
