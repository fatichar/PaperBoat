using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    public class TextLine : IEnumerable<TextBlock>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        public int TextBlockCount { get; }
        public int WordCount { get; }
        public int CharCount { get; }
        #endregion

        #region private properties
        private IReadOnlyList<TextBlock> TextBlocks { get; }
        private int Offset { get; }
#if DEBUG            
        public string Text { get; }
#endif
        #endregion

        #region Constructors
        public TextLine(IReadOnlyList<TextBlock> textBlocks, int offset, int textBlockCount)
        {
            TextBlocks = textBlocks;

            if (offset < 0 || offset >= textBlocks.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textBlocks.Count - 1);
            }
            if (textBlockCount < 1 || offset + textBlockCount > textBlocks.Count)
            {
                throw new ArgumentOutOfRangeException(textBlockCount, nameof(textBlockCount), 1, textBlocks.Count - offset);
            }

            Offset = offset;

            TextBlockCount = textBlockCount;
            WordCount = TextBlocks.Sum(b => b.WordCount);
            CharCount = TextBlocks.Sum(b => b.CharCount) + TextBlockCount - 1;

            Rect = Geometry.GetUnion(textBlocks);
#if DEBUG
            Text = textBlocks.Skip(offset).Take(TextBlockCount).Select(tb => tb.ToString()).Aggregate((tb1, tb2) => tb1 + "    " + tb2);
#endif
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

        public static bool CanEndAt(char ch)
        {
            return ch == '\n';
        }
        #endregion

#if DEBUG
        public override string ToString() => Text;
#endif
    }
}
