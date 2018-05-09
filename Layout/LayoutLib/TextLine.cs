using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace LayoutLib
{
    public class TextLine : IEnumerable<TextBlock>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        [PublicAPI]
        public int TextBlockCount { get; }
        [PublicAPI]
        public int WordCount { get; }
        [PublicAPI]
        public int CharCount { get; }
        #endregion

        #region private properties
        private ImmutableList<TextBlock> TextBlocks { get; }
        private int Offset { get; }
#if DEBUG
        private string Text { get; }
#endif
        #endregion

        #region Constructors
        public TextLine(ImmutableList<TextBlock> textBlocks, int offset, int textBlockCount)
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
                return TextBlocks[Offset + index];
            }
        }
        #endregion

        #region IEnumerable Impl
        public IEnumerator<TextBlock> GetEnumerator() => TextBlocks.GetRange(Offset, TextBlockCount).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => TextBlocks.GetRange(Offset, TextBlockCount).GetEnumerator();
        #endregion

        [PublicAPI]
        public static bool CanEndAt(char ch)
        {
            return ch == '\n';
        }
#if DEBUG
        public override string ToString() => Text;
#endif
    }
}
