using PDFLib;
using System;
using System.Linq;

namespace PDFApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine();
            var doc = Reader.Read("TestData\\Admission Offer Letter.pdf");
            Console.WriteLine(doc.Pages.First().ToString());
        }
    }
}
