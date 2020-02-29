using System.Drawing;

namespace ImageFiltering
{
    public class ConvolutionProcessor
    {

        private readonly Bitmap _map;

        public ConvolutionProcessor(Bitmap bitmap)
        {
            _map = bitmap;
        }

        public Bitmap ComputeWith(Filter filter)
        {
            var result = new Bitmap(_map.Width, _map.Height);

            var offset = filter.Size / 2;
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    var colorMap = new Color[filter.Size, filter.Size];

                    for (int filterY = 0; filterY < filter.Size; filterY++)
                    {
                        int pk = (filterY + x - offset <= 0) ? 0 :
                            (filterY + x - offset >= _map.Width - 1) ? _map.Width - 1 : filterY + x - offset;
                        for (int filterX = 0; filterX < filter.Size; filterX++)
                        {
                            int pl = (filterX + y - offset <= 0) ? 0 :
                                (filterX + y - offset >= _map.Height - 1) ? _map.Height - 1 : filterX + y - offset;

                            colorMap[filterY, filterX] = _map.GetPixel(pk, pl);
                        }
                    }

                    result.SetPixel(x, y, colorMap * filter);
                }
            }

            return result;
        }
    }
}
