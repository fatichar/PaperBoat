using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace LayoutLib
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

#if DEBUG
        private string Text { get; }
#endif
        #endregion

        #region Constructors
        public Page(ImmutableArray<Character> chars, ImmutableArray<Word> words, ImmutableArray<TextBlock> blocks, ImmutableArray<TextLine> textLines, Size size)
        {
            Chars = chars;
            Words = words;
            TextBlocks = blocks;
            TextLines = textLines;

            Rect = new Rectangle(new Point(0, 0), size);
            TextRect = Geometry.GetUnion(textLines);
#if DEBUG
            Text = textLines.Select(tb => tb.ToString()).Aggregate((tb1, tb2) => tb1 + "\n" + tb2);
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
