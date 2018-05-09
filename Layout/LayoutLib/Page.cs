using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace LayoutLib
{
    public class Page : IEnumerable<TextLine>, IRect
    {
        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        [PublicAPI]
        public int TextLineCount  => TextLines.Count;
        [PublicAPI]
        public int TextBlockCount => TextBlocks.Count;
        [PublicAPI]
        public int WordCount      => Words.Count;
        [PublicAPI]
        public int CharCount      => Chars.Count;
        [PublicAPI]
        public Rectangle TextRect { get; }
        #endregion

        #region private properties
        //private int Offset { get; }
        private ImmutableList<Character> Chars { get; }
        private ImmutableList<Word> Words { get; }
        private ImmutableList<TextBlock> TextBlocks { get; }
        private ImmutableList<TextLine> TextLines { get; }

#if DEBUG
        private string Text { get; }
#endif
        #endregion

        #region Constructors
        //public Page(Size size, ImmutableList<TextLine> textLines, int offset, int textLineCount)
        //{
        //    TextLines = textLines;

        //    if (offset < 0 || offset >= textLines.Count)
        //    {
        //        throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textLines.Count - 1);
        //    }
        //    if (textLineCount < 1 || offset + textLineCount >= textLines.Count)
        //    {
        //        throw new ArgumentOutOfRangeException(textLineCount, nameof(textLineCount), 1, textLines.Count - offset);
        //    }

        //    Offset = offset;
        //    TextLineCount = textLineCount;

        //    TextRect = Geometry.GetUnion(textLines);
        //    Rect = new Rectangle(new Point(0, 0), size);
        //}

        public Page(Size size, string text, ImmutableList<Character> chars, ImmutableList<Word> words, ImmutableList<TextBlock> blocks, ImmutableList<TextLine> textLines)
        {
            Text          = text;
            Chars         = chars;
            Words         = words;
            TextBlocks    = blocks;
            TextLines     = textLines;

            Rect      = new Rectangle(new Point(0, 0), size);
            TextRect  = Geometry.GetUnion(textLines);
#if DEBUG            
            Text = words.Select(tl => tl.ToString()).Aggregate((tl1, tl2) => tl1 + "\n" + tl2);
#endif
        }
        #endregion

        #region public methods
        public TextLine this[int index]
        {
            get
            {
                if (index < 0 || index >= TextLineCount)
                {
                    throw new ArgumentOutOfRangeException(index, nameof(index), 0, TextLineCount - 1);
                }
                return TextLines[index];
            }
        }
        #endregion

        #region IEnumerable Impl
        public IEnumerator<TextLine> GetEnumerator() => TextLines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => TextLines.GetEnumerator();
        #endregion

#if DEBUG            
        public override string ToString() => Text;
#endif
    }
}
