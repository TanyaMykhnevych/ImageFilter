using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageFiltering
{
    class LocalBitmap
    {
        public Bitmap Bitmap { get; set; }
        public String Name { get; set; }

        public LocalBitmap(String name, Bitmap bitmap)
        {
            Name = name;
            Bitmap = bitmap;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<LocalBitmap> sourceBitmaps = new List<LocalBitmap>();

            string[] images = Directory.GetFiles(@"..\..\..\Source\");
            images.ToList().ForEach(i =>
            {
                Image img = Image.FromFile(i);
                sourceBitmaps.Add(new LocalBitmap(Path.GetFileName(i), new Bitmap(img)));
            });


            var filter = new GaussianBlurFilter();

            sourceBitmaps.ForEach(b =>
            {
                ConvolutionProcessor processor = new ConvolutionProcessor(b.Bitmap);
                Bitmap result = processor.ComputeWith(filter);
                result.Save($@"..\..\..\Processed\{b.Name}", b.Bitmap.RawFormat);

            });

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
