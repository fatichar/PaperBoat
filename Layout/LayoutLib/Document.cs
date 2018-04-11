using System.Collections.Generic;

namespace LayoutLib
{
    public class Document
    {
        #region public properties
        public int PageCount => Pages.Count;
        #endregion

        #region private properties
        private IReadOnlyList<Page> Pages { get; }
        #endregion

        #region constructor
        public Document(IReadOnlyList<Page> pages)
        {
            Pages = pages;
        }
        #endregion
    }
}
