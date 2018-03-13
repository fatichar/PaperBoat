using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LayoutLib
{
    class Page : IEnumerable<TextLine>, IRect
    {
        #region public properties
        public int TextLineCount { get; }
        public Rectangle TextRect { get; }
        #endregion

        #region private properties
        private IReadOnlyList<TextLine> TextLines { get; }
        private int Offset { get; }
        private Rectangle Rect { get; }
        #endregion

        #region Constructors
        public Page(Size size, IReadOnlyList<TextLine> textLines, int offset, int textLineCount)
        {
            TextLines = textLines;

            if (offset < 0 || offset >= textLines.Count)
            {
                throw new ArgumentOutOfRangeException(offset, nameof(offset), 0, textLines.Count - 1);
            }
            if (textLineCount < 1 || offset + textLineCount >= textLines.Count)
            {
                throw new ArgumentOutOfRangeException(textLineCount, nameof(textLineCount), 1, textLines.Count - offset);
            }

            Offset = offset;
            TextLineCount = textLineCount;

            TextRect = Geometry.GetUnion(textLines);
            Rect = new Rectangle(new Point(0, 0), size);
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

        #region IRect Impl
        public Rectangle GetRect() => Rect;
        #endregion
    }
}
