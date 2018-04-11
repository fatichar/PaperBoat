using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayoutLib
{
    public class Page : IEnumerable<TextLine>, IRect
    {
        private object textLines;

        #region IRect Impl
        public Rectangle Rect { get; }
        #endregion

        #region public properties
        public int TextLineCount  => TextLines.Count;
        public int TextBlockCount => TextBlocks.Count;
        public int WordCount      => Words.Count;
        public int CharCount      => Chars.Count;
        public Rectangle TextRect { get; }
        #endregion

        #region private properties
        private int Offset { get; }
        private IReadOnlyList<Character> Chars { get; }
        private IReadOnlyList<Word> Words { get; }
        private IReadOnlyList<TextBlock> TextBlocks { get; }
        private IReadOnlyList<TextLine> TextLines { get; }

#if DEBUG            
        public string Text { get; }
#endif
        #endregion

        #region Constructors
        //public Page(Size size, IReadOnlyList<TextLine> textLines, int offset, int textLineCount)
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

        public Page(Size size, string text, IReadOnlyList<Character> chars, IReadOnlyList<Word> words, IReadOnlyList<TextBlock> blocks, IReadOnlyList<TextLine> textLines)
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
