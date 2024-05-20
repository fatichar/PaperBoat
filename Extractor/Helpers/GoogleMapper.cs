using Google.Cloud.DocumentAI.V1;
using PaperBoat.Model;
using static Extractor.Helpers.ProtoExtensions;
using ValueType = PaperBoat.Model.ValueType;

namespace Extractor.Helpers;

public static class GoogleMapper
{
    internal static Extract ConvertDocument(Document googleDoc)
    {
        var groups = googleDoc.Entities
            .Where(entity => entity.Confidence > 0.8)
            .Select(CreateFieldFromEntity)
            .Select(field => CreateGroup(field.Name, new List<Field> { field }, field.Confidence, field.Rect))
            .ToList();

        var document = CreateExtract("", groups);
        return document;
    }

    private static Field CreateFieldFromEntity(Document.Types.Entity entity)
    {
        var value = entity.MentionText ?? "";
        var type = entity.Type ?? "";
        var rectangle = new Rectangle();
        if (entity.PageAnchor != null && entity.PageAnchor.PageRefs.Count > 0)
        {
            rectangle = GetRectangleFromPolygon(entity.PageAnchor.PageRefs[0].BoundingPoly);
        }
        return new Field
        {
            Name = type,
            ValueType = ValueType.String,
            Value = value,
            Confidence = ToConfidence(entity.Confidence),
            Rect = rectangle
        };
    }

    private static Rectangle GetRectangleFromPolygon(BoundingPoly boundingPoly)
    {
        throw new NotImplementedException();
    }

    private static int ToConfidence(float? confidence)
    {
        if (confidence == null) return 0;

        return (int)(confidence * 100);
    }
}