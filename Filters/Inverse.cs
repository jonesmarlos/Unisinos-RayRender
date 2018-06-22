using RayRender.Interfaces;
using RayRender.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Filters
{
    public class Inverse : IFilter
    {
        public void Execute(IWorld parameter)
        {
            Logger.Debug("Filter Inverse");

            IImage image = parameter.Image;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    IRGBColor color = image.GetColor(x, y);

                    color.Red = 1.0f - color.Red;
                    color.Green = 1.0f - color.Green;
                    color.Blue = 1.0f - color.Blue;

                    image.SetColor(x, y, color);
                }
            }
        }

        public void Parse(Parameters parameters)
        {
            
        }
    }
}
