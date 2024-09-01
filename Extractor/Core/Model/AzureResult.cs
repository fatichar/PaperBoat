namespace PaperBoat.Extractor.Core.Model.Azure;

public class AzureResult
{
    public string Status { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime LastUpdatedDateTime { get; set; }
    public Analyzeresult AnalyzeResult { get; set; }
}

public class Analyzeresult
{
    public string ApiVersion { get; set; }
    public string ModelId { get; set; }
    public string StringIndexType { get; set; }
    public string Content { get; set; }
    public Page[] Pages { get; set; }
    public Table[] Tables { get; set; }
    public Paragraph[] Paragraphs { get; set; }
    public Keyvaluepair[] KeyValuePairs { get; set; }
    public object[] Entities { get; set; }
    public Style[] Styles { get; set; }
    public object[] Documents { get; set; }
}

public class Page
{
    public int PageNumber { get; set; }
    public float Angle { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Unit { get; set; }
    public Word[] Words { get; set; }
    public Selectionmark[] SelectionMarks { get; set; }
    public Line[] Lines { get; set; }
    public Span[] Spans { get; set; }
}

public class Word
{
    public string Content { get; set; }
    public int[] Polygon { get; set; }
    public float Confidence { get; set; }
    public Span Span { get; set; }
}

public class Span
{
    public int Offset { get; set; }
    public int Length { get; set; }
}

public class Selectionmark
{
    public string State { get; set; }
    public int[] Polygon { get; set; }
    public float Confidence { get; set; }
    public Span Span { get; set; }
}

public class Line
{
    public string Content { get; set; }
    public int[] Polygon { get; set; }
    public Span[] Spans { get; set; }
}

public class Table
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public Cell[] Cells { get; set; }
    public Boundingregion[] BoundingRegions { get; set; }
    public Span[] Spans { get; set; }
}

public class Cell
{
    public string Kind { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public string Content { get; set; }
    public Boundingregion[] BoundingRegions { get; set; }
    public Span[] Spans { get; set; }
    public int ColumnSpan { get; set; }
    public int RowSpan { get; set; }
}

public class Boundingregion
{
    public int PageNumber { get; set; }
    public int[] Polygon { get; set; }
}

public class Paragraph
{
    public Span[] Spans { get; set; }
    public Boundingregion[] BoundingRegions { get; set; }
    public string Role { get; set; }
    public string Content { get; set; }
}

public class Keyvaluepair
{
    public Key Key { get; set; }
    public Value Value { get; set; }
    public float Confidence { get; set; }
}

public class Key
{
    public string Content { get; set; }
    public Boundingregion[] BoundingRegions { get; set; }
    public Span[] Spans { get; set; }
}

public class Value
{
    public string Content { get; set; }
    public Boundingregion[] BoundingRegions { get; set; }
    public Span[] Spans { get; set; }
}

public class Style
{
    public float Confidence { get; set; }
    public Span[] Spans { get; set; }
    public bool IsHandwritten { get; set; }
}