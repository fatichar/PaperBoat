using PDFLib;
using System;

namespace PDFApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = Reader.Read("d:\\data\\1.pdf");
            Console.WriteLine("Hello World!");
        }
    }
}
