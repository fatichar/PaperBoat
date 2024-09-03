using System.Drawing;
using Azure.AI.DocumentIntelligence;
using Serilog;
using static PaperBoat.Extractor.Helpers.ProtoExtensions;

namespace PaperBoat.Extractor.Azure;

public class InClassName
{
    public InClassName(AnalyzedDocument azureDoc, string docType)
    {
        AzureDoc = azureDoc;
        DocType = docType;
    }

    public AnalyzedDocument AzureDoc { get; private set; }
    public string DocType { get; private set; }
}

public static class AzureMapper
{
    internal static Extract ConvertDocument(InClassName inClassName)
    {
        var groups = new List<Group>();
        var nameFields = new List<Field>();

        if (inClassName.AzureDoc.Fields.TryGetValue("VendorName", out var vendorNameField)
            && vendorNameField.Type == DocumentFieldType.String)
        {
            nameFields.Add(CreateFieldFromDocumentField(vendorNameField, "VendorName"));
            Console.WriteLine(
                $"Vendor Name: '{vendorNameField.ValueString}', with confidence {vendorNameField.Confidence}");
        }

        if (inClassName.AzureDoc.Fields.TryGetValue("CustomerName", out var customerNameField)
            && customerNameField.Type == DocumentFieldType.String)
        {
            nameFields.Add(CreateFieldFromDocumentField(customerNameField, "CustomerName"));
            Log.Information($"Customer Name: '{customerNameField.ValueString}', with confidence {customerNameField.Confidence}");
        }

        var rectangle = GetRectangleForFieldsGroup(nameFields);

        groups.Add(CreateGroup("Names", nameFields, 0));

        if (inClassName.AzureDoc.Fields.TryGetValue("Items", out var itemsField)
            && itemsField.Type == DocumentFieldType.Array)
        {
            var itemCount = 0;
            foreach (var itemDocumentField in itemsField.ValueArray)
            {
                ++itemCount;
                Console.WriteLine("Item:");
                var itemFields = new List<Field>();

                if (itemDocumentField.Type != DocumentFieldType.Object) continue;

                IReadOnlyDictionary<string, DocumentField> itemDocumentFields = itemDocumentField.ValueObject;

                if (itemDocumentFields.TryGetValue("Description", out var itemDescriptionField)
                    && itemDescriptionField.Type == DocumentFieldType.String)
                {
                    itemFields.Add(CreateFieldFromDocumentField(itemDescriptionField, "Description"));
                    Console.WriteLine(
                        $"  Description: '{itemDescriptionField.ValueString}', with confidence {itemDescriptionField.Confidence}");
                }

                if (itemDocumentFields.TryGetValue("Amount", out var itemAmountField)
                    && itemAmountField.Type == DocumentFieldType.Currency)
                {
                    itemFields.Add(CreateAmountFieldFromDocumentField(itemAmountField, "Amount"));
                    if (itemAmountField.ValueCurrency != null)
                    {
                        var itemAmount = itemAmountField.ValueCurrency;
                        Console.WriteLine($"  Amount: '{itemAmount.CurrencySymbol}" +
                                          $"{itemAmount.Amount}', with confidence {itemAmountField.Confidence}");
                    }
                }

                groups.Add(CreateGroup("Item" + itemCount, itemFields, 0));
            }
        }

        var totalFields = new List<Field>();

        if (inClassName.AzureDoc.Fields.TryGetValue("SubTotal", out var subTotalField)
            && subTotalField.Type == DocumentFieldType.Currency)
        {
            totalFields.Add(CreateAmountFieldFromDocumentField(subTotalField, "SubTotal"));
            if (subTotalField.ValueCurrency != null)
            {
                var subTotal = subTotalField.ValueCurrency;
                Console.WriteLine($"Sub Total: '{subTotal.CurrencySymbol}{subTotal.Amount}', " +
                                  $"with confidence {subTotalField.Confidence}");
            }
        }

        if (inClassName.AzureDoc.Fields.TryGetValue("TotalTax", out var totalTaxField)
            && totalTaxField.Type == DocumentFieldType.Currency)
        {
            totalFields.Add(CreateAmountFieldFromDocumentField(totalTaxField, "TotalTax"));
            if (totalTaxField.ValueCurrency != null)
            {
                var totalTax = totalTaxField.ValueCurrency;
                Console.WriteLine($"Total Tax: '{totalTax.CurrencySymbol}{totalTax.Amount}', " +
                                  $"with confidence {totalTaxField.Confidence}");
            }
        }

        if (inClassName.AzureDoc.Fields.TryGetValue("InvoiceTotal", out var invoiceTotalField)
            && invoiceTotalField.Type == DocumentFieldType.Currency)
        {
            totalFields.Add(CreateAmountFieldFromDocumentField(invoiceTotalField, "InvoiceTotal"));
            if (invoiceTotalField.ValueCurrency != null)
            {
                var invoiceTotal = invoiceTotalField.ValueCurrency;
                Console.WriteLine($"Invoice Total: '{invoiceTotal.CurrencySymbol}{invoiceTotal.Amount}', " +
                                  $"with confidence {invoiceTotalField.Confidence}");
            }
        }

        rectangle = GetRectangleForFieldsGroup(totalFields);
        groups.Add(CreateGroup("Total", totalFields, 0));

        var document = CreateExtract(inClassName.DocType, groups);
        return document;
    }

    private static Field CreateAmountFieldFromDocumentField(DocumentField amountField, string name)
    {
        var snippet = new Snippet(0, GetRectangleFromPolygon(amountField.BoundingRegions));

        var valueText = amountField.ValueCurrency == null
            ? amountField.Content
            : amountField.ValueCurrency.CurrencySymbol + amountField.ValueCurrency.Amount;
        var value = new PaperBoat.Value(valueText, valueText, ToConfidence(amountField.Confidence));

        return new Field(name, ValueType.Currency, snippet)
        {
            Value = value,
            Confidence = ToConfidence(amountField.Confidence)
        };
    }

    private static byte ToConfidence(float? confidence)
    {
        return confidence == null ? (byte)0 : (byte)(confidence * 100);
    }

    private static Field CreateFieldFromDocumentField(DocumentField docField, string name)
    {
        var rectangle = GetRectangleFromPolygon(docField.BoundingRegions);
        var snippet = new Snippet(0, rectangle);
        var value = new PaperBoat.Value(docField.Content, docField.Content, ToConfidence(docField.Confidence));

        var field = new Field(name, ValueType.String, snippet)
        {
            Confidence = ToConfidence(docField.Confidence),
            Value = value
        };

        return field;
    }

    private static Rectangle GetRectangleFromPolygon(IReadOnlyList<BoundingRegion> regions)
    {
        if (regions.Count == 0)
        {
            return Rectangle.Empty;
        }

        var rects = regions.Select(r => GetBoundingRect(r.Polygon));

        return rects.Aggregate(Rectangle.Union);
    }

    private static Rectangle GetRectangleForFieldsGroup(IReadOnlyList<Field> fields)
    {
        if (fields.Count == 0)
        {
            return Rectangle.Empty;
        }

        var rects = fields.Select((Field f) => f.Snippet.Rect);
        var groupRectangle = rects.Aggregate(Rectangle.Union);

        return groupRectangle;
    }

    private static Rectangle GetBoundingRect(IReadOnlyList<float> polygon)
    {
        var top = float.MaxValue;
        var left = float.MaxValue;
        var bottom = float.MinValue;
        var right = float.MinValue;

        for (var i = 0; i < polygon.Count; ++i)
        {
            var position = polygon[i];

            if (i % 2 == 0) //x coordinate
            {
                if (left > position)
                {
                    left = position;
                }
                if (right < position)
                {
                    right = position;
                }
            }
            else
            {
                if (top > position)
                {
                    top = position;
                }

                if (bottom < position)
                {
                    bottom = position;
                }
            }
        }

        return new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
    }
}