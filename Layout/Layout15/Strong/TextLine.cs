using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace Layout15.Strong
{
    public class TextLine : TextLine<TextBlock, Word, Character>, IRect
    {
        #region Constructors
        public TextLine(ImmutableArray<TextBlock> textBlocks, int offset, int textBlockCount) 
            : 
            base(textBlocks, offset, textBlockCount)
        {
            Rect = Geometry.GetUnion(textBlocks);
        }
        #endregion
        
        // IRect Impl
        public Rectangle Rect { get; }
    }
}
