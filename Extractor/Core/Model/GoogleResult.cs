namespace PaperBoat.Extractor.Core.Model.Google;

public class GoogleResult
{
    public string Uri { get; set; }
    public bool HasUri { get; set; }
    public object[] Content { get; set; }
    public bool HasContent { get; set; }
    public string MimeType { get; set; }
    public string Text { get; set; }
    public object[] TextStyles { get; set; }
    public Page[] Pages { get; set; }
    public Entity[] Entities { get; set; }
    public object[] EntityRelations { get; set; }
    public object[] TextChanges { get; set; }
    public object ShardInfo { get; set; }
    public object Error { get; set; }
    public object[] Revisions { get; set; }
    public int SourceCase { get; set; }
}

public class Page
{
    public int PageNumber { get; set; }
    public Image Image { get; set; }
    public object[] Transforms { get; set; }
    public Dimension Dimension { get; set; }
    public Layout Layout { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
    public Block[] Blocks { get; set; }
    public Paragraph[] Paragraphs { get; set; }
    public Line[] Lines { get; set; }
    public Token[] Tokens { get; set; }
    public VisualElement[] VisualElements { get; set; }
    public Table[] Tables { get; set; }
    public FormField[] FormFields { get; set; }
    public object[] Symbols { get; set; }
    public object[] DetectedBarcodes { get; set; }
    public object ImageQualityScores { get; set; }
    public object Provenance { get; set; }
}

public class Image
{
    public int[] Content { get; set; }
    public string MimeType { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Dimension
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string Unit { get; set; }
}

public class Layout
{
    public TextAnchor TextAnchor { get; set; }
    public float Confidence { get; set; }
    public BoundingPoly BoundingPoly { get; set; }
    public int Orientation { get; set; }
}

public class TextAnchor
{
    public TextSegment[] TextSegments { get; set; }
    public string Content { get; set; }
}

public class TextSegment
{
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
}

public class BoundingPoly
{
    public Vertex[] Vertices { get; set; }
    public NormalizedVertex[] NormalizedVertices { get; set; }
}

public class Vertex
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class NormalizedVertex
{
    public float X { get; set; }
    public float Y { get; set; }
}

public class DetectedLanguage
{
    public string LanguageCode { get; set; }
    public float Confidence { get; set; }
}

public class Block
{
    public Layout Layout { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
    public object Provenance { get; set; }
}

public class Paragraph
{
    public Layout Layout { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
    public object Provenance { get; set; }
}

public class Line
{
    public Layout Layout { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
    public object Provenance { get; set; }
}

public class Token
{
    public Layout Layout { get; set; }
    public DetectedBreak DetectedBreak { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
    public object Provenance { get; set; }
    public object StyleInfo { get; set; }
}

public class DetectedBreak
{
    public int Type { get; set; }
}

public class VisualElement
{
    public Layout Layout { get; set; }
    public string Type { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
}

public class Table
{
    public Layout Layout { get; set; }
    public HeaderRow[] HeaderRows { get; set; }
    public object[] BodyRows { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
    public object Provenance { get; set; }
}

public class HeaderRow
{
    public Cell[] Cells { get; set; }
}

public class Cell
{
    public Layout Layout { get; set; }
    public int RowSpan { get; set; }
    public int ColSpan { get; set; }
    public DetectedLanguage[] DetectedLanguages { get; set; }
}

public class FormField
{
    public FieldName FieldName { get; set; }
    public FieldValue FieldValue { get; set; }
    public DetectedLanguage[] NameDetectedLanguages { get; set; }
    public DetectedLanguage[] ValueDetectedLanguages { get; set; }
    public string ValueType { get; set; }
    public string CorrectedKeyText { get; set; }
    public string CorrectedValueText { get; set; }
    public object Provenance { get; set; }
}

public class FieldName
{
    public TextAnchor TextAnchor { get; set; }
    public float Confidence { get; set; }
    public BoundingPoly BoundingPoly { get; set; }
    public int Orientation { get; set; }
}

public class FieldValue
{
    public TextAnchor TextAnchor { get; set; }
    public float Confidence { get; set; }
    public BoundingPoly BoundingPoly { get; set; }
    public int Orientation { get; set; }
}

public class Entity
{
    public TextAnchor TextAnchor { get; set; }
    public string Type { get; set; }
    public string MentionText { get; set; }
    public string MentionId { get; set; }
    public float Confidence { get; set; }
    public PageAnchor PageAnchor { get; set; }
    public string Id { get; set; }
    public object NormalizedValue { get; set; }
    public Property[] Properties { get; set; }
    public object Provenance { get; set; }
    public bool Redacted { get; set; }
}

public class PageAnchor
{
    public PageRef[] PageRefs { get; set; }
}

public class PageRef
{
    public int Page { get; set; }
    public int LayoutType { get; set; }
    public string LayoutId { get; set; }
    public BoundingPoly BoundingPoly { get; set; }
    public float Confidence { get; set; }
}

public class Property
{
    public TextAnchor TextAnchor { get; set; }
    public string Type { get; set; }
    public string MentionText { get; set; }
    public string MentionId { get; set; }
    public float Confidence { get; set; }
    public PageAnchor PageAnchor { get; set; }
    public string Id { get; set; }
    public object NormalizedValue { get; set; }
    public Property[] Properties { get; set; }
    public object Provenance { get; set; }
    public bool Redacted { get; set; }
}