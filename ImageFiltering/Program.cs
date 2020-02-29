using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageFiltering
{
    class Program
    {
        static void Main(string[] args)
        {
            var filter = new GaussianBlurFilter();

            string[] images = Directory.GetFiles(@"..\..\..\Source\");
            images.ToList().ForEach(i =>
            {
                Image img = Image.FromFile(i);
                Bitmap bpm = new Bitmap(img);
                ConvolutionProcessor processor = new ConvolutionProcessor(bpm);
                Bitmap result = processor.ComputeWith(filter);
                result.Save($@"..\..\..\Processed\{Path.GetFileName(i)}", bpm.RawFormat);
            });

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
