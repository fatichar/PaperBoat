using Azure.AI.DocumentIntelligence;
using PaperBoat.Model;
using Field = PaperBoat.Model.Field;
using Group = PaperBoat.Model.Group;
using Rectangle = PaperBoat.Model.Rectangle;
using ValueType = PaperBoat.Model.ValueType;

namespace Extractor.Helpers;

public class AzureDocumentObjectConverter
{
    internal static void ConvertDocument(AnalyzedDocument azureDoc,
        string docType,
        out Extract document)
    {
        List<Group> groups = new List<Group>();
        List<Field> nameFields = new List<Field>();

        if (azureDoc.Fields.TryGetValue("VendorName", out var vendorNameField)
            && vendorNameField.Type == DocumentFieldType.String)
        {
            nameFields.Add(CreateFieldFromDocumentField(vendorNameField, "VendorName"));
            Console.WriteLine(
                $"Vendor Name: '{vendorNameField.ValueString}', with confidence {vendorNameField.Confidence}");
        }

        if (azureDoc.Fields.TryGetValue("CustomerName", out var customerNameField)
            && customerNameField.Type == DocumentFieldType.String)
        {
            nameFields.Add(CreateFieldFromDocumentField(customerNameField, "CustomerName"));
            Console.WriteLine(
                $"Customer Name: '{customerNameField.ValueString}', with confidence {customerNameField.Confidence}");
        }

        var rectangle = GetRectangleForFieldsGroup(nameFields);

        groups.Add(ProtoExtensions.CreateGroup("Names", nameFields, 0, rectangle));

        if (azureDoc.Fields.TryGetValue("Items", out var itemsField)
            && itemsField.Type == DocumentFieldType.Array)
        {
            var itemCount = 0;
            foreach (var itemDocumentField in itemsField.ValueArray)
            {
                ++itemCount;
                Console.WriteLine("Item:");
                List<Field> itemFields = new List<Field>();

                if (itemDocumentField.Type == DocumentFieldType.Object)
                {
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
                            Console.WriteLine($"  Amount: '{itemAmountField.ValueCurrency.CurrencySymbol}" +
                                              $"{itemAmountField.ValueCurrency.Amount}', with confidence {itemAmountField.Confidence}");
                        }
                    }

                    rectangle = GetRectangleForFieldsGroup(itemFields);

                    groups.Add(ProtoExtensions.CreateGroup("Item" + itemCount, itemFields, 0, rectangle));
                }
            }
        }

        List<Field> totalFields = new List<Field>();

        if (azureDoc.Fields.TryGetValue("SubTotal", out var subTotalField)
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

        if (azureDoc.Fields.TryGetValue("TotalTax", out var totalTaxField)
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

        if (azureDoc.Fields.TryGetValue("InvoiceTotal", out var invoiceTotalField)
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
        groups.Add(ProtoExtensions.CreateGroup("Total", totalFields, 0, rectangle));

        document = ProtoExtensions.CreateExtract(docType, groups);
    }

    private static Field CreateAmountFieldFromDocumentField(DocumentField amountField, string name)
    {
        var rectangle = GetRectangleFromPolygon(amountField);

        var value = amountField.ValueCurrency == null
            ? amountField.Content
            : amountField.ValueCurrency.CurrencySymbol + amountField.ValueCurrency.Amount;

        return new Field
        {
            Name = name,
            ValueType = ValueType.String,
            Value = value,
            Confidence = ToConfidence(amountField.Confidence),
            Rect = rectangle
        };
    }

    private static int ToConfidence(float? confidence)
    {
        if (confidence == null) return 0;

        return (int)(confidence * 100);
    }

    private static Rectangle GetRectangleForFieldsGroup(List<Field> fields)
    {
        if (fields.Count == 0)
        {
            return new Rectangle();
        }

        var groupRectangle = fields[0].Rect;

        for (var i = 1; i < fields.Count; i++)
        {
            groupRectangle = GetRectanglesUnion(groupRectangle, fields[i].Rect);
        }

        return groupRectangle;
    }

    private static Rectangle GetRectanglesUnion(Rectangle groupRectangle, Rectangle rect)
    {
        if (rect.IsEmpty())
        {
            return groupRectangle;
        }

        if (groupRectangle.IsEmpty())
        {
            return rect;
        }

        var top = groupRectangle.Top < rect.Top ? groupRectangle.Top : rect.Top;
        var bottom = groupRectangle.Bottom > rect.Bottom ? groupRectangle.Bottom : rect.Bottom;
        var left = groupRectangle.Left < rect.Left ? groupRectangle.Left : rect.Left;
        var right = groupRectangle.Right > rect.Right ? groupRectangle.Right : rect.Right;

        return new Rectangle(left, top, right - left, bottom - top);
    }

    private static Field CreateFieldFromDocumentField(DocumentField docField, string name)
    {
        var rectangle = GetRectangleFromPolygon(docField);

        var field = new Field(name, "string")
        {
            Value = docField.ValueString,
            Confidence = (int)docField.Confidence,
            Rect = rectangle
        };

        return field;
    }

    private static Rectangle GetRectangleFromPolygon(DocumentField docField)
    {
        if (docField.BoundingRegions.Count == 0)
        {
            return Rectangle.Empty;
        }

        var top = float.MaxValue;
        var left = float.MaxValue;
        var bottom = float.MinValue;
        var right = float.MinValue;

        for (var i = 0; i < docField.BoundingRegions[0].Polygon.Count; ++i)
        {
            var position = docField.BoundingRegions[0].Polygon[i];

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

        var rectangle = new Rectangle((int)left,
            (int)top, (int)(right - left),
            (int)(bottom - top));

        return rectangle;
    }
}