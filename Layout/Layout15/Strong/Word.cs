using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

namespace Layout15.Strong
{
    public class Word : Word<Character>, IRect
    {
        #region Constructors
        public Word(ImmutableArray<Character> chars, int offset, int charCount)
            :
            base(chars, offset, charCount)
        {
            Rect = Geometry.GetUnion(chars);
        }
        #endregion
        
        // IRect Impl
        public Rectangle Rect { get; }
    }
}
