using System.Drawing;
using Google.Cloud.DocumentAI.V1;
using static Google.Cloud.DocumentAI.V1.Document.Types;

namespace PaperBoat.Extractor.Extractors;

//This needs to be made generic to extract group snippet based on anchors
internal class AnchorBasedExtractor(Field field, Document document, Rectangle rect)
{
    public void Extract()
    {
        List<Entity> entities = GetAllEntitiesInRect(rect);


    }

    private List<Entity> GetAllEntitiesInRect(Rectangle snippet)
    {
        throw new NotImplementedException();
    }
}