using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace LayoutLib
{
    class TextLine : IEnumerable<TextBlock>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        public int TextBlockCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<TextBlock> TextBlocks { get; }
        private int Offset { get; }
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

            Offset = offset;
            TextBlockCount = textBlockCount;

            Rect = Geometry.GetUnion(textBlocks);
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
    }
}
