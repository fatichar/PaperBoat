using System.Drawing;
using Google.Cloud.DocumentAI.V1;

using static PaperBoat.Extractor.Helpers.ProtoExtensions;

namespace PaperBoat.Extractor.Google;

public static class GoogleMapper
{
    internal static Extract ConvertDocument(Document googleDoc)
    {
        var groups = googleDoc.Entities
            //.Where(entity => entity.Confidence > 0.8)
            .Select(CreateFieldFromEntity)
            .Select(field =>
                CreateGroup(
                    field.Name,
                    new List<Field> { field },
                    field.Value?.Confidence ?? 0))
            .ToList();

        var document = CreateExtract("", groups);
        return document;
    }

    private static Field CreateFieldFromEntity(Document.Types.Entity entity)
    {
        var valueText = entity.MentionText ?? "";
        var name = entity.Type ?? "";
        var rectangle = new Rectangle();
        if (entity.PageAnchor != null && entity.PageAnchor.PageRefs.Count > 0)
        {
            rectangle = GetRectangleFromPolygon(entity.PageAnchor.PageRefs[0].BoundingPoly);
        }
        var snippet = new Snippet(0, rectangle);
        var value = new PaperBoat.Value(valueText, valueText, ToConfidence(entity.Confidence));

        return new Field(name, ValueType.String, snippet)
        {
            Value = value
        };
    }

    private static Rectangle GetRectangleFromPolygon(global::Google.Cloud.DocumentAI.V1.BoundingPoly boundingPoly)
    {
        int left = 0;
        int right = 0;
        int top = 0;
        int bottom = 0;

        foreach (var vertex in boundingPoly.Vertices)
        {
            if (left > vertex.X)
            {
                left = vertex.X;
            }

            if (right < vertex.X)
            {
                right = vertex.X;
            }

            if (top > vertex.Y)
            {
                top = vertex.Y;
            }

            if (bottom < vertex.Y)
            {
                bottom = vertex.Y;
            }
        }

        return new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
    }

    private static byte ToConfidence(float? confidence)
    {
        if (confidence == null) return 0;

        return (byte)(confidence * 100);
    }
}