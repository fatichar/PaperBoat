namespace RestApi.Helpers
{
    public class AzureDocumentObjectConverter
    {
        internal static void ConvertDocument(Google.Cloud.DocumentAI.V1.Document googleDoc, out Models.Document document)
        {
            document = new Models.Document("", new List<Models.Group>());
        }
    }
}
