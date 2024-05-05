﻿using RestApi.Controllers;
using RestApi.Models;
using System.Drawing;

using Azure;
using Azure.AI.DocumentIntelligence;
using System.Security.Cryptography;
using System.Buffers.Text;

namespace RestApi.Services.DataExtractor
{
    public class AzureDataExtractor : IDataExtractor
    {
        private string endpoint = "https://testextraction2.cognitiveservices.azure.com/";
        private string key = "258957c923c74144a08715a9063bdba2";
        private ExtractionRequest extractionRequest;

        public AzureDataExtractor(ExtractionRequest extractionRequest)
        {
            this.extractionRequest = extractionRequest;
        }

        public Document Extract(string content, string docType, ExtractionRequest extractionRequest)
        {
            return ProcessDocument(content).Result;            
        }

        private async Task<Document> ProcessDocument(string content)
        {
            AzureKeyCredential credential = new AzureKeyCredential(key);
            DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
            string docType = "prebuilt-invoice";

            //sample invoice document

            byte[] imgData = File.ReadAllBytes("D:\\Data\\ADE\\bill.jpeg");

            System.BinaryData binaryData = BinaryData.FromBytes(imgData);

            //Uri invoiceUri = new Uri("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-invoice.pdf");
            //Uri invoiceUri = new Uri("D:\\Data\\ADE\\bill.jpeg");

            AnalyzeDocumentContent docContent = new AnalyzeDocumentContent()
            {
                Base64Source = binaryData
            };

            Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, docType, docContent);

            AnalyzeResult result = operation.Value;

            for (int i = 0; i < result.Documents.Count; i++)
            {
                Console.WriteLine($"Document {i}:");

                AnalyzedDocument analyzedDocument = result.Documents[i];

                List<Group> groups = new List<Group>();
                List<Field> nameFields = new List<Field>();

                if (analyzedDocument.Fields.TryGetValue("VendorName", out DocumentField vendorNameField)
                    && vendorNameField.Type == DocumentFieldType.String)
                {
                    nameFields.Add(CreateFieldFromDocumentField(vendorNameField, "VendorName"));                    
                    Console.WriteLine($"Vendor Name: '{vendorNameField.ValueString}', with confidence {vendorNameField.Confidence}");
                }

                if (analyzedDocument.Fields.TryGetValue("CustomerName", out DocumentField customerNameField)
                    && customerNameField.Type == DocumentFieldType.String)
                {
                    nameFields.Add(CreateFieldFromDocumentField(customerNameField, "CustomerName"));
                    Console.WriteLine($"Customer Name: '{customerNameField.ValueString}', with confidence {customerNameField.Confidence}");
                }

                Rectangle rectangle = GetRectangleForFieldsGroup(nameFields);

                groups.Add(new Group("Names", nameFields, 0, rectangle));

                if (analyzedDocument.Fields.TryGetValue("Items", out DocumentField itemsField)
                    && itemsField.Type == DocumentFieldType.Array)
                {
                    int itemCount = 0;
                    foreach (DocumentField itemDocumentField in itemsField.ValueArray)
                    {
                        ++itemCount;
                        Console.WriteLine("Item:");
                        List<Field> itemFields = new List<Field>();
                        
                        if (itemDocumentField.Type == DocumentFieldType.Object)
                        {
                            IReadOnlyDictionary<string, DocumentField> itemDocumentFields = itemDocumentField.ValueObject;

                            if (itemDocumentFields.TryGetValue("Description", out DocumentField itemDescriptionField)
                                && itemDescriptionField.Type == DocumentFieldType.String)
                            {
                                itemFields.Add(CreateFieldFromDocumentField(itemDescriptionField, "Description"));
                                Console.WriteLine($"  Description: '{itemDescriptionField.ValueString}', with confidence {itemDescriptionField.Confidence}");
                            }

                            if (itemDocumentFields.TryGetValue("Amount", out DocumentField itemAmountField)
                                && itemAmountField.Type == DocumentFieldType.Currency)
                            {
                                itemFields.Add(CreateAmountFieldFromDocumentField(itemAmountField, "Amount"));
                                if (itemAmountField.ValueCurrency != null)
                                {
                                    CurrencyValue itemAmount = itemAmountField.ValueCurrency;
                                    Console.WriteLine($"  Amount: '{itemAmountField.ValueCurrency.CurrencySymbol}" +
                                        $"{itemAmountField.ValueCurrency.Amount}', with confidence {itemAmountField.Confidence}");
                                }                                
                            }

                            rectangle = GetRectangleForFieldsGroup(itemFields);

                            groups.Add(new Group("Item" + itemCount, itemFields, 0, rectangle));
                        }
                    }
                }

                List<Field> totalFields = new List<Field>();


                if (analyzedDocument.Fields.TryGetValue("SubTotal", out DocumentField subTotalField)
                    && subTotalField.Type == DocumentFieldType.Currency)
                {
                    totalFields.Add(CreateAmountFieldFromDocumentField(subTotalField, "SubTotal"));
                    if (subTotalField.ValueCurrency != null)
                    {
                        CurrencyValue subTotal = subTotalField.ValueCurrency;
                        Console.WriteLine($"Sub Total: '{subTotal.CurrencySymbol}{subTotal.Amount}', " +
                            $"with confidence {subTotalField.Confidence}");
                    }
                }

                if (analyzedDocument.Fields.TryGetValue("TotalTax", out DocumentField totalTaxField)
                    && totalTaxField.Type == DocumentFieldType.Currency)
                {
                    totalFields.Add(CreateAmountFieldFromDocumentField(totalTaxField, "TotalTax"));
                    if (totalTaxField.ValueCurrency != null)
                    {
                        CurrencyValue totalTax = totalTaxField.ValueCurrency;
                        Console.WriteLine($"Total Tax: '{totalTax.CurrencySymbol}{totalTax.Amount}', " +
                            $"with confidence {totalTaxField.Confidence}");
                    }
                }

                if (analyzedDocument.Fields.TryGetValue("InvoiceTotal", out DocumentField invoiceTotalField)
                    && invoiceTotalField.Type == DocumentFieldType.Currency)
                {
                    totalFields.Add(CreateAmountFieldFromDocumentField(invoiceTotalField, "InvoiceTotal"));
                    if (invoiceTotalField.ValueCurrency != null)
                    {
                        CurrencyValue invoiceTotal = invoiceTotalField.ValueCurrency;
                        Console.WriteLine($"Invoice Total: '{invoiceTotal.CurrencySymbol}{invoiceTotal.Amount}', " +
                            $"with confidence {invoiceTotalField.Confidence}");
                    }
                }

                rectangle = GetRectangleForFieldsGroup(totalFields);
                groups.Add(new Group("Total", totalFields, 0, rectangle));

                Document document = new Document(docType, groups);
                return document;
            }
            return null;
        }

        private Field CreateAmountFieldFromDocumentField(DocumentField amountField, string name)
        {
            Rectangle rectangle = GetRectangleFromPolygon(amountField);

            if (amountField.ValueCurrency != null)
            {
                return new Field(name, "string")
                {
                    Value = amountField.ValueCurrency.CurrencySymbol + amountField.ValueCurrency.Amount,
                    Confidence = (int)amountField.Confidence,
                    Rect = rectangle
                };
            }
            else
            {
                return new Field(name, "string")
                {
                    Value = amountField.Content,
                    Confidence = (int)amountField.Confidence,
                    Rect = rectangle
                };
            }
        }

        private Rectangle GetRectangleForFieldsGroup(List<Field> fields)
        {
            if (fields.Count == 0)
            {
                return new Rectangle();
            }

            Rectangle groupRectangle = fields[0].Rect;

            for (int i = 1; i < fields.Count; i++)
            {
                groupRectangle = GetRectanglesUnion(groupRectangle, fields[i].Rect);
            }

            return groupRectangle;
        }

        private Rectangle GetRectanglesUnion(Rectangle groupRectangle, Rectangle rect)
        {
            if (rect.IsEmpty)
            {
                return groupRectangle;
            }
            
            if (groupRectangle.IsEmpty)
            {
                return rect;
            }

            int top = groupRectangle.Top < rect.Top ? groupRectangle.Top : rect.Top;
            int bottom = groupRectangle.Bottom > rect.Bottom ? groupRectangle.Bottom : rect.Bottom;
            int left = groupRectangle.Left < rect.Left ? groupRectangle.Left : rect.Left;
            int right = groupRectangle.Right > rect.Right ? groupRectangle.Right : rect.Right;

            return new Rectangle(left, top, right - left, bottom - top);
        }

        private Field CreateFieldFromDocumentField(DocumentField docField, string name)
        {
            Rectangle rectangle = GetRectangleFromPolygon(docField);

            Field field = new Field(name, "string")
            {
                Value = docField.ValueString,
                Confidence = (int)docField.Confidence,
                Rect = rectangle
            };

            return field;
        }

        private Rectangle GetRectangleFromPolygon(DocumentField docField)
        {
            if (docField.BoundingRegions.Count == 0)
            {
                return Rectangle.Empty;
            }

            float top = float.MaxValue;
            float left = float.MaxValue;
            float bottom = float.MinValue;
            float right = float.MinValue;

            for (int i = 0; i < docField.BoundingRegions[0].Polygon.Count; ++i)
            {
                float position = docField.BoundingRegions[0].Polygon[i];

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

            Rectangle rectangle = new Rectangle((int)left,
                (int)top, (int)(right - left), 
                (int) (bottom - top));

            return rectangle;
        }
    }
}
