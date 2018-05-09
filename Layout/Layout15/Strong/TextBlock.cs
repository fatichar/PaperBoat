using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace Layout15.Strong
{
    public class TextBlock : TextBlock<Word, Character>, IRect
    {
        #region Constructors
        public TextBlock(ImmutableArray<Word> words, int offset, int wordCount) 
            : 
            base(words, offset, wordCount)
        {
            Rect = Geometry.GetUnion(words);
        }
        #endregion
        
        // IRect Impl
        public Rectangle Rect { get; }
    }
}
