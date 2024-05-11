
using Google.Cloud.DocumentAI.V1;
using RestApi.Models;
using System.Drawing;

namespace RestApi.Helpers
{
    public class GoogleDocumentObjectConverter
    {
        internal static void ConvertDocument(Google.Cloud.DocumentAI.V1.Document googleDoc, out Models.Document document)
        {
            var groups = new List<Models.Group>();

            foreach (var entity in googleDoc.Entities)
            {
                if (entity.Confidence > 0.8)
                {
                    Models.Field field = CreateFieldFromEntity(entity);
                    Group group = new Group(field.Name, 
                        new List<Field> { field }, 
                        field.Confidence,
                        field.Rect);

                    groups.Add(group);
                }
            }

            document = new Models.Document("", groups);            
        }

        private static Field CreateFieldFromEntity(Google.Cloud.DocumentAI.V1.Document.Types.Entity entity)
        {
            string value = entity.MentionText;
            string type = entity.Type;
            Rectangle rectangle = Rectangle.Empty;
            if (entity.PageAnchor != null && entity.PageAnchor.PageRefs.Count > 0)
            {
                rectangle = GetRectangleFromPolygon(entity.PageAnchor.PageRefs[0].BoundingPoly);
            }   
            float confidence = entity.Confidence;
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
}
