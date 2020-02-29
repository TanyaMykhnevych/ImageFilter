using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageFiltering
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] images = Directory.GetFiles(@"..\..\..\Source\", "*", SearchOption.AllDirectories);

            Stopwatch s = new Stopwatch();
            s.Start();

            List<Thread> threads = new List<Thread>();

            threads.Add(new Thread(() => ApplyGaussian3x3(images)));
            threads.Add(new Thread(() => ApplyGaussian5x5(images)));
            threads.Add(new Thread(() => ApplyBoxBlur(images)));
            threads.Add(new Thread(() => ApplyMean5x5(images)));
            threads.Add(new Thread(() => ApplySharpen3x3(images)));
            threads.Add(new Thread(() => ApplySharpen5x5(images)));
            threads.Add(new Thread(() => ApplySharpenIntence(images)));

            threads.ForEach(s => s.Start());
            threads.ForEach(s => s.Join());

            s.Stop();

            Console.WriteLine($"Done {TimeSpan.FromMilliseconds(s.ElapsedMilliseconds).TotalMinutes}");
            Console.ReadLine();
        }

        static void ApplyGaussian3x3(string[] images)
        {
            Gaussian3x3BlurFilter gaussian3x3 = new Gaussian3x3BlurFilter();
            ApplyFilter(images, gaussian3x3);
        }

        static void ApplyGaussian5x5(string[] images)
        {
            Gaussian5x5BlurFilter gaussian5x5 = new Gaussian5x5BlurFilter();
            ApplyFilter(images, gaussian5x5);
        }

        static void ApplyBoxBlur(string[] images)
        {
            BoxBlurFilter boxBlur = new BoxBlurFilter();
            ApplyFilter(images, boxBlur);
        }

        static void ApplyMean5x5(string[] images)
        {
            Mean5x5BlurFilter mean5x5 = new Mean5x5BlurFilter();
            ApplyFilter(images, mean5x5);
        }

        static void ApplySharpen3x3(string[] images)
        {
            Sharpen3x3Filter sharpen3x3 = new Sharpen3x3Filter();
            ApplyFilter(images, sharpen3x3);
        }

        static void ApplySharpen5x5(string[] images)
        {
            Sharpen5x5Filter sharpen5x5 = new Sharpen5x5Filter();
            ApplyFilter(images, sharpen5x5);
        }

        static void ApplySharpenIntence(string[] images)
        {
            SharpenIntenseFilter sharpenIntence = new SharpenIntenseFilter();
            ApplyFilter(images, sharpenIntence);
        }

        static void ApplyFilter(string[] images, Filter filter)
        {
            Parallel.ForEach(images, (i) =>
            {
                Bitmap bpm = new Bitmap(Image.FromFile(i));
                ConvolutionProcessor processor = new ConvolutionProcessor(bpm);
                Bitmap result = processor.ComputeWith(filter);
                string newPath = Path.Combine(@"..\..\..\Processed", filter.ToString(), Path.GetFileName(i));

                string dirName = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);

                result.Save(newPath, bpm.RawFormat);
            });
        }
    }
}
