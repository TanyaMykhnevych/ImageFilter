using System;
using System.Drawing;

namespace ImageFiltering
{
    abstract public class Filter
    {
        abstract public double[,] Data { get; }

        public int Size => (int)Math.Sqrt(Data.Length);

        public double this[int i, int j] => Data[i, j];

        abstract public int NormalizationRate { get; }

        abstract public int Bias { get; }

        public static Color operator *(Color[,] map, Filter filter)
        {
            if ((int)Math.Sqrt(map.Length) != filter.Size)
                throw new ArgumentException("Different sizes in multiplication");

            double red = 0;
            double green = 0;
            double blue = 0;

            for (int y = 0; y < filter.Size; y++)
            {
                for (int x = 0; x < filter.Size; x++)
                {
                    red += map[y, x].R * filter[y, x];
                    green += map[y, x].G * filter[y, x];
                    blue += map[y, x].B * filter[y, x];
                }
            }


            return Color.FromArgb(Normalize(red, filter), Normalize(green, filter), Normalize(blue, filter));
        }

        protected static int Normalize(double value, Filter filter)
        {
            return Math.Min(Math.Max((int)(value / filter.NormalizationRate + filter.Bias), 0), 255);
        }
    }

    #region Blur

    public class Gaussian3x3BlurFilter : Filter
    {
        public override int NormalizationRate { get; } = 16;

        public override int Bias { get; } = 0;

        public override double[,] Data { get; } = {
            { 1, 2, 1, },
            { 2, 4, 2, },
            { 1, 2, 1, },
        };

        public override string ToString()
        {
            return "Gaussian3x3Blur";
        }
    }

    public class Gaussian5x5BlurFilter : Filter
    {
        public override int NormalizationRate { get; } = 256;

        public override int Bias { get; } = 0;

        public override double[,] Data { get; } = {
            { 1, 4, 6, 4, 1 },
            { 4, 16, 24, 16, 4 },
            { 6, 24, 36, 24, 6 },
            { 4, 16, 24, 16, 4 },
            { 1, 4, 6, 4, 1 }
        };

        public override string ToString()
        {
            return "Gaussian5x5Blur";
        }
    }

    public class Mean5x5BlurFilter : Filter
    {
        public override int NormalizationRate { get; } = 25;

        public override int Bias { get; } = 0;

        public override double[,] Data { get; } =  { {  1, 1, 1, 1, 1 },
                                                     {  1, 1, 1, 1, 1 },
                                                     {  1, 1, 1, 1, 1 },
                                                     {  1, 1, 1, 1, 1 },
                                                     {  1, 1, 1, 1, 1 }, };

        public override string ToString()
        {
            return "Mean5x5Blur";
        }
    }
    #endregion Blur

    #region Sharpen
    public class Sharpen3x3Filter : Filter
    {
        public override int NormalizationRate { get; } = 3;

        public override int Bias { get; } = 0;

        public override double[,] Data { get; } = {
           {  0, -2,  0, },
           { -2, 11, -2, },
           {  0, -2,  0, },
        };

        public override string ToString()
        {
            return "Sharpen3x3";
        }
    }

    public class SharpenIntenseFilter : Filter
    {
        public override int NormalizationRate { get; } = 1;

        public override int Bias { get; } = 0;

        public override double[,] Data { get; } = {
            { 1,  1,  1, },
            { 1, -7,  1, },
            { 1,  1,  1, },
        };

        public override string ToString()
        {
            return "SharpenIntence";
        }
    }

    public class Sharpen5x5Filter : Filter
    {
        public override int NormalizationRate { get; } = 6;

        public override int Bias { get; } = 0;

        public override double[,] Data { get; } = {
           { -1, -1, -1, -1, -1, },
           { -1,  2,  2,  2, -1, },
           { -1,  2,  7,  2, -1, },
           { -1,  2,  2,  2, -1, },
           { -1, -1, -1, -1, -1, },
        };

        public override string ToString()
        {
            return "Sharpen5x5";
        }
    }
    #endregion Sharpen
}

