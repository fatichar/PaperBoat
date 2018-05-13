using PDFLib;
using System;
using System.Linq;

namespace PDFApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var doc = Reader.Read("d:\\data\\1.pdf");
            Console.WriteLine(doc.Pages.First().ToString());
        }
    }
}
