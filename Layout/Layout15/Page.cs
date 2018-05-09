using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace Layout15
{
    public class Page<TLine, TBlock, TWord, TChar> :  IRect where TLine : TextLine<TBlock, TWord, TChar> where TBlock : TextBlock<TWord, TChar> where TWord : Word<TChar>
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        [PublicAPI]
        public int TextLineCount  => TextLines.Length;
        [PublicAPI]
        public int TextBlockCount => TextBlocks.Length;
        [PublicAPI]
        public int WordCount      => Words.Length;
        [PublicAPI]
        public int CharCount      => Chars.Length;
        [PublicAPI]
        public Rectangle TextRect { get; }
        #endregion

        #region private properties
        //private int Offset { get; }
        private ImmutableArray<TChar> Chars { get; }
        private ImmutableArray<TWord> Words { get; }
        private ImmutableArray<TBlock> TextBlocks { get; }
        private ImmutableArray<TLine> TextLines { get; }

#if DEBUG
        private string Text { get; }
#endif
        #endregion

        #region Constructors
        //public Page(Size size, ImmutableArray<TextLine> textLines, int offset, int textLineCount)
        //{
        //    TextLines = textLines;

        //    if (offset < 0 || offset >= textLines.Length)
        //    {
        //        throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textLines.Length - 1);
        //    }
        //    if (textLineCount < 1 || offset + textLineCount >= textLines.Length)
        //    {
        //        throw new ArgumentOutOfRangeException(textLineCount, nameof(textLineCount), 1, textLines.Length - offset);
        //    }

        //    Offset = offset;
        //    TextLineCount = textLineCount;

        //    TextRect = Geometry.GetUnion(textLines);
        //    Rect = new Rectangle(new Point(0, 0), size);
        //}

        public Page(Size size, string text, ImmutableArray<TChar> chars, ImmutableArray<TWord> words, ImmutableArray<TBlock> blocks, ImmutableArray<TLine> textLines)
        {
            Text          = text;
            Chars         = chars;
            Words         = words;
            TextBlocks    = blocks;
            TextLines     = textLines;

            Rect      = new Rectangle(new Point(0, 0), size);
#if DEBUG            
            Text = words.Select(tl => tl.ToString()).Aggregate((tl1, tl2) => tl1 + "\n" + tl2);
#endif
        }
        #endregion

        #region public methods
        public TLine this[int index] => TextLines[index];
        #endregion

#if DEBUG            
        public override string ToString() => Text;
#endif
    }
}
