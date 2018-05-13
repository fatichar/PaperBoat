using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using LayoutLib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.IO;

namespace PDFLib
{
    public static class Reader
    {
        public static LayoutLib.Document Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("", filePath);
            }
            
            var reader = new PdfReader(filePath);
            var document = new PdfDocument(reader);

            var pageCount = document.GetNumberOfPages();

            var pdfPages = new List<PdfPage>();

            for (var i = 1; i <= pageCount; i++)
            {
                var page = document.GetPage(i);
                pdfPages.Add(page);
            }

            var pageLayouts = pdfPages.ConvertAll(CreatePage).ToImmutableArray();

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

        private static LayoutLib.Page CreatePage(System.Drawing.Size size, string text)
        {
            var chars = CreateCharacters(text);
            var _words = new List<Word>();

            var wordStarted = false;
            var wordStart = 0;

            for (var i = 0; i < chars.Length; i++)
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
                    if (!Word.CanEndAt(ch)) continue;

                    var word = new Word(chars, wordStart, i - wordStart);
                    _words.Add(word);
                    wordStarted = false;
                }
            }

            var words = _words.ToImmutableArray();
            var blocks = CreateBlocks(words);
            var textLines = CreateTextLines(blocks);

            var page = new LayoutLib.Page(chars, words, blocks, textLines, size);

            return page;
        }

        private static ImmutableArray<TextBlock> CreateBlocks(ImmutableArray<Word> words)
        {
            var blocksList = new List<TextBlock>();

            var blockStarted = false;
            var blockStart = 0;

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];

                if (!blockStarted)
                {
                    blockStarted = true;
                    blockStart = i;
                }

                var wordEndChar = word.EndChar?.Value ?? '\n';
                if (!TextBlock.CanEndAt(wordEndChar)) continue;

                var block = new TextBlock(words, blockStart, 1 + i - blockStart);
                blocksList.Add(block);
                blockStarted = false;
            }

            if (blockStarted)
            {
                var block = new TextBlock(words, blockStart, words.Length - blockStart);
                blocksList.Add(block);
            }

            return blocksList.ToImmutableArray();
        }

        private static ImmutableArray<TextLine> CreateTextLines(ImmutableArray<TextBlock> blocks)
        {
            var linesList = new List<TextLine>();

            var lineStarted = false;
            var lineStart = 0;

            for (var i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];

                if (!lineStarted)
                {
                    lineStarted = true;
                    lineStart = i;
                }

                var blockEndChar = block.EndChar?.Value ?? '\n';
                if (!TextLine.CanEndAt(blockEndChar)) continue;

                var line = new TextLine(blocks, lineStart, 1 + i - lineStart);
                linesList.Add(line);
                lineStarted = false;
            }

            if (lineStarted)
            {
                var line = new TextLine(blocks, lineStart, blocks.Length - lineStart);
                linesList.Add(line);
            }

            return linesList.ToImmutableArray();
        }
        
        private static ImmutableArray<Character> CreateCharacters(string text)
        {
            var characters = new List<LayoutLib.Character>(text.Length);

            for (var i = 0; i < text.Length; i++)
            {
                var ch = new Character(text, i, System.Drawing.Rectangle.Empty);
                characters.Add(ch);
            }

            return characters.ToImmutableArray();
        }
    }
}
