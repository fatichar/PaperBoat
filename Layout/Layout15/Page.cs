using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace Layout15
{
    public class Page : IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        [PublicAPI]
        public int TextLineCount => TextLines.Length;
        [PublicAPI]
        public int TextBlockCount => TextBlocks.Length;
        [PublicAPI]
        public int WordCount => Words.Length;
        [PublicAPI]
        public int CharCount => Chars.Length;
        [PublicAPI]
        public Rectangle TextRect { get; }
        #endregion

        #region private properties
        //private int Offset { get; }
        private ImmutableArray<Character> Chars { get; }
        private ImmutableArray<Word> Words { get; }
        private ImmutableArray<TextBlock> TextBlocks { get; }
        private ImmutableArray<TextLine> TextLines { get; }
        protected readonly string Text = "";

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

        public Page(Size size, string text, ImmutableArray<Character> chars, ImmutableArray<Word> words, ImmutableArray<TextBlock> blocks, ImmutableArray<TextLine> textLines)
        {
            Chars = chars;
            Words = words;
            TextBlocks = blocks;
            TextLines = textLines;

            Rect = new Rectangle(new Point(0, 0), size);
            TextRect = Geometry.GetUnion(textLines);
#if DEBUG            
            Text = text;
#endif
        }
        #endregion

        #region public methods
        public TextLine this[int index] => TextLines[index];
        #endregion

#if DEBUG            
        public override string ToString() => Text;
#endif
    }
}
