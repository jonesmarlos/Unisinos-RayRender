using RayRender.Interfaces;
using RayRender.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Filters
{
    public class GrayScale : IFilter
    {
        public void Execute(IWorld parameter)
        {
            Logger.Debug("Filter GrayScale");

            IImage image = parameter.Image;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    IColor color = image.GetColor(x, y);

                    float grayScale = (color.Red + color.Green + color.Blue) / 3.0f;

                    color.Red = grayScale;
                    color.Green = grayScale;
                    color.Blue = grayScale;

                    image.SetColor(x, y, color);
                }
            }
        }

        public void Parse(Parameters parameters)
        {

        }
    }
}
