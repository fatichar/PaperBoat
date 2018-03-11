using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LayoutLib
{
    class TextLine : IEnumerable<TextBlock>, IRect
    {
        #region public properties
        public int TextBlockCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<TextBlock> TextBlocks { get; }
        private int Offset { get; }
        private Rectangle Rect { get; }
        #endregion

        #region Constructors
        public TextLine(IReadOnlyList<TextBlock> textBlocks, int offset, int textBlockCount)
        {
            TextBlocks = textBlocks;

            if (offset < 0 || offset >= textBlocks.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textBlocks.Count - 1);
            }
            if (textBlockCount < 1 || offset + textBlockCount >= textBlocks.Count)
            {
                throw new ArgumentOutOfRangeException(textBlockCount, nameof(textBlockCount), 1, textBlocks.Count - offset);
            }

            TextBlockCount = textBlockCount;
        }
        #endregion

        #region public methods
        public TextBlock this[int index]
        {
            get
            {
                if (index < 0 || index >= TextBlockCount)
                {
                    throw new ArgumentOutOfRangeException(index, nameof(index), 0, TextBlockCount - 1);
                }
                return TextBlocks[index];
            }
        }
        #endregion

        #region IEnumerable Impl
        public IEnumerator<TextBlock> GetEnumerator() => TextBlocks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => TextBlocks.GetEnumerator();
        #endregion

        #region IRect Impl
        public Rectangle GetRect() => Rect;
        #endregion
    }
}
