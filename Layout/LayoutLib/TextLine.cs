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
        // public properties
        [PublicAPI]
        public int TexTextBlockCount => TextBlocks.Length;
        [PublicAPI]
        public int WordCount { get; }
        [PublicAPI]
        public int CharCount { get; }

        #region protected properties

        protected readonly ArraySlice<TextBlock> TextBlocks;
#if DEBUG
        protected readonly string Text = "";
#endif
        #endregion

        #region Constructors
        public TextLine(ImmutableArray<TextBlock> textBlocks, int offset, int texTextBlockCount)
            :
            this(textBlocks, offset, texTextBlockCount, Rectangle.Empty)
        {
            Rect = Geometry.GetUnion(this.TextBlocks);
        }

        public TextLine(ImmutableArray<TextBlock> textBlocks, int offset, int texTextBlockCount, Rectangle rect)
        {
            if (offset < 0 || offset >= textBlocks.Length)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textBlocks.Length - 1);
            }
            if (texTextBlockCount < 1 || offset + texTextBlockCount > textBlocks.Length)
            {
                throw new ArgumentOutOfRangeException(texTextBlockCount, nameof(texTextBlockCount), 1, textBlocks.Length - offset);
            }
            
            TextBlocks = new ArraySlice<TextBlock>(textBlocks, offset, texTextBlockCount);

            Rect = rect;

            WordCount = TextBlocks.Sum(b => b.WordCount);
            CharCount = TextBlocks.Sum(b => b.CharCount) + TexTextBlockCount - 1;

#if DEBUG
            Text = textBlocks.Skip(offset).Take(TexTextBlockCount).Select(tb => tb.ToString()).Aggregate((tb1, tb2) => tb1 + "    " + tb2);
#endif
        }
        #endregion

        #region public methods
        public TextBlock this[int index] => TextBlocks[index];

        [PublicAPI]
        public static bool CanEndAt(char ch)
        {
            return ch == '\n';
        }

#if DEBUG
        public override string ToString() => Text;
#endif
        #endregion

        #region IEnumerable Impl
        public IEnumerator<TextBlock> GetEnumerator() => TextBlocks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)TextBlocks).GetEnumerator();
        #endregion

        // IRect Impl
        public Rectangle Rect { get; }
    }
}
