using System.Drawing;
using Google.Cloud.DocumentAI.V1;
using PaperBoat.Model;

namespace Extractor.Helpers;

public class GoogleDocumentObjectConverter
{
    internal static void ConvertDocument(Google.Cloud.DocumentAI.V1.Document googleDoc, out Extract document)
    {
        var groups = new List<Group>();

        foreach (var entity in googleDoc.Entities)
        {
            if (entity.Confidence > 0.8)
            {
                var field = CreateFieldFromEntity(entity);
                var group = new Group(field.Name, 
                    new List<Field> { field }, 
                    field.Confidence,
                    field.Rect);

                groups.Add(group);
            }
        }

        document = new Extract("", groups);            
    }

    private static Field CreateFieldFromEntity(Google.Cloud.DocumentAI.V1.Document.Types.Entity entity)
    {
        var value = entity.MentionText;
        var type = entity.Type;
        var rectangle = Rectangle.Empty;
        if (entity.PageAnchor != null && entity.PageAnchor.PageRefs.Count > 0)
        {
            rectangle = GetRectangleFromPolygon(entity.PageAnchor.PageRefs[0].BoundingPoly);
        }   
        var confidence = entity.Confidence;
        return new Field(entity.Type, entity.Type)
        {
            Value = value,
            Rect = rectangle,
            Confidence = (int)Math.Ceiling(confidence * 100)
        };
    }

    private static Rectangle GetRectangleFromPolygon(BoundingPoly boundingPoly)
    {
        throw new NotImplementedException();
    }
}