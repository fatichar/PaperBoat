using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout;
using LayoutLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PDFLib
{
    public class Reader
    {
        public static LayoutLib.Document Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("", filePath);
            }

            var reader = new PdfReader(filePath);
            var document = new PdfDocument(reader);

            int pageCount = document.GetNumberOfPages();

            var pdfPages = new List<PdfPage>();

            for (int i = 1; i <= pageCount; i++)
            {
                var page = document.GetPage(i);
                pdfPages.Add(page);
            }

            var pageLayouts = pdfPages.ConvertAll(CreatePage);

            return new LayoutLib.Document(pageLayouts);
        }

        private static LayoutLib.Page CreatePage(PdfPage pdfPage)
        {
            var text = PdfTextExtractor.GetTextFromPage(pdfPage, new LocationTextExtractionStrategy());

            var pageWidth = (int) pdfPage.GetPageSize().GetWidth();
            var pageHeight = (int) pdfPage.GetPageSize().GetHeight();

            var pageSize = new Size(pageWidth, pageHeight);
            var page = CreatePage(pageSize, text);

            return page;
        }

        private static LayoutLib.Page CreatePage(Size size, string text)
        {
            var chars     = CreateCharacters(text);
            var words     = new List<Word>();
            var blocks    = new List<TextBlock>();
            var textLines = new List<TextLine>();

            var wordStarted = false;

            var wordStart  = 0;
            var blockStart = 0;
            var lineStart  = 0;

            for (int i = 0; i < chars.Count; i++)
            {
                var ch = chars[i].Value;

                if (!wordStarted)
                {
                    if (Word.CanStartWith(ch))
                    {
                        wordStarted = true;
                        wordStart = i;
                    }
                }
                else
                {
                    if (Word.CanEndAt(ch))
                    {
                        var word = new Word(chars, wordStart, i - wordStart);                        
                        words.Add(word);
                        wordStarted = false;

                        if (TextBlock.CanEndAt(ch))
                        {
                            var block = new TextBlock(words, blockStart, words.Count - blockStart);
                            blocks.Add(block);
                            blockStart = words.Count;

                            if (TextLine.CanEndAt(ch))
                            {
                                var line = new TextLine(blocks, lineStart, blocks.Count - lineStart);
                                textLines.Add(line);
                                lineStart = textLines.Count;
                            }
                        }
                    }
                }
            }

            var page = new LayoutLib.Page(size, text, chars, words, blocks, textLines);

            return page;
        }

        private static IReadOnlyList<LayoutLib.Character> CreateCharacters(string text)
        {
            var characters = new List<LayoutLib.Character>(text.Length);

            for (int i = 0; i < text.Length; i++)
            {
                var ch = new Character(text, i, Rectangle.Empty);
                characters.Add(ch);
            }

            return characters;
        }
    }
}
