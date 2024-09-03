namespace PaperBoat;

using System.Drawing;

public record ExtractionRequest(string FileType, string DocType, string Content);

public record ExtractionResponse(Extract? Doc, ExtractionError? Error, DateTime Timestamp);

public record Extract(IReadOnlyList<Group> Groups, int TotalPages)
{
    public Extract() : this([], 0)
    {
    }
}

public record Group(
    string Name,
    IReadOnlyList<Field> Fields,
    byte Confidence
    )
{
    public IReadOnlyList<Snippet> Snippets { get; init; }
}

public record Field(string Name, ValueType ValueType, Snippet Snippet)
{
    public Value? Value { get; init; }
    public IReadOnlyList<RawChar>? RawText { get; init; }
}

public record Value(
    string ExtractedValue,
    string FormattedValue,
    byte Confidence
)
{
    public IReadOnlyList<RawChar>? RawText { get; init; }
}

public record Snippet(int PageNumber, Rectangle Rect)
{
    public byte Confidence { get; init; }
}

public record RawChar(string Value, byte Confidence, Rectangle Rect);

public enum ValueType
{
    String,
    Integer,
    Decimal,
    Currency,
    Percentage,
    Date,
    Time,
    Boolean,
    AccountNumber,
    Aadhar,
    PAN,
    Enum
}

public record ExtractionError(string Message, ErrorType ErrorType);

public enum ErrorType
{
    UnknownError,
    InternalError,
    InvalidInput,
    EmptyDocument,
    FileTooLarge,
    UnsupportedDocType,
    UnsupportedFileFormat,
    EncryptionError,
    PoorQuality,
    ParsingError,
    OperationTimeout,
    ServiceUnavailable,
    InsufficientPermissions,
    RateLimitExceeded,
    NetworkError
}