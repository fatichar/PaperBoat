using System.Collections.Generic;
using System.Collections.Immutable;

namespace LayoutLib
{
    public class Document
    {
        #region public properties
        public int PageCount => Pages.Count;
        #endregion

        #region private properties
        private ImmutableList<Page> Pages { get; }
        #endregion

        #region constructor
        public Document(ImmutableList<Page> pages)
        {
            Pages = pages;
        }
        #endregion
    }
}
