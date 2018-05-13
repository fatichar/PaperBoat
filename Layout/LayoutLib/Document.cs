using System.Collections.Immutable;
using JetBrains.Annotations;

namespace LayoutLib
{
    public class Document
    {
        #region public properties
        [PublicAPI]
        public int PageCount => Pages.Length;
        [PublicAPI]
        public ImmutableArray<Page> Pages { get; }
        #endregion

        #region private properties
        #endregion

        #region constructor
        [PublicAPI]
        public Document(ImmutableArray<Page> pages)
        {
            Pages = pages;
        }
        #endregion
    }
}
