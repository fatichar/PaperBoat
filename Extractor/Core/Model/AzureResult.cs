using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperBoat.Extractor.Core.Model.Azure
{
    public class AzureResult
    {
        public string status { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastUpdatedDateTime { get; set; }
        public Analyzeresult analyzeResult { get; set; }
    }

    public class Analyzeresult
    {
        public string apiVersion { get; set; }
        public string modelId { get; set; }
        public string stringIndexType { get; set; }
        public string content { get; set; }
        public Page[] pages { get; set; }
        public Table[] tables { get; set; }
        public Paragraph[] paragraphs { get; set; }
        public Keyvaluepair[] keyValuePairs { get; set; }
        public object[] entities { get; set; }
        public Style[] styles { get; set; }
        public object[] documents { get; set; }
    }

    public class Page
    {
        public int pageNumber { get; set; }
        public float angle { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string unit { get; set; }
        public Word[] words { get; set; }
        public Selectionmark[] selectionMarks { get; set; }
        public Line[] lines { get; set; }
        public Span[] spans { get; set; }
    }

    public class Word
    {
        public string content { get; set; }
        public int[] polygon { get; set; }
        public float confidence { get; set; }
        public Span span { get; set; }
    }

    public class Span
    {
        public int offset { get; set; }
        public int length { get; set; }
    }

    public class Selectionmark
    {
        public string state { get; set; }
        public int[] polygon { get; set; }
        public float confidence { get; set; }
        public Span span { get; set; }
    }

    public class Line
    {
        public string content { get; set; }
        public int[] polygon { get; set; }
        public Span[] spans { get; set; }
    }

    public class Table
    {
        public int rowCount { get; set; }
        public int columnCount { get; set; }
        public Cell[] cells { get; set; }
        public Boundingregion[] boundingRegions { get; set; }
        public Span[] spans { get; set; }
    }

    public class Cell
    {
        public string kind { get; set; }
        public int rowIndex { get; set; }
        public int columnIndex { get; set; }
        public string content { get; set; }
        public Boundingregion[] boundingRegions { get; set; }
        public Span[] spans { get; set; }
        public int columnSpan { get; set; }
        public int rowSpan { get; set; }
    }

    public class Boundingregion
    {
        public int pageNumber { get; set; }
        public int[] polygon { get; set; }
    }

    public class Paragraph
    {
        public Span[] spans { get; set; }
        public Boundingregion[] boundingRegions { get; set; }
        public string role { get; set; }
        public string content { get; set; }
    }

    public class Keyvaluepair
    {
        public Key key { get; set; }
        public Value value { get; set; }
        public float confidence { get; set; }
    }

    public class Key
    {
        public string content { get; set; }
        public Boundingregion[] boundingRegions { get; set; }
        public Span[] spans { get; set; }
    }

    public class Value
    {
        public string content { get; set; }
        public Boundingregion[] boundingRegions { get; set; }
        public Span[] spans { get; set; }
    }

    public class Style
    {
        public float confidence { get; set; }
        public Span[] spans { get; set; }
        public bool isHandwritten { get; set; }
    }
}