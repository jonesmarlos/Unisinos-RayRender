using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Filters
{
    public class ToonFilter : IFilter
    {
        public void Execute(IWorld parameter)
        {
            IImage originalImage = parameter.Image;

            ConvolutionFilter edgeFilter = new ConvolutionFilter();
            edgeFilter.Factor = 1.0f;
            edgeFilter.Size = 3;
            edgeFilter.Kernel = new float[3, 3]{
                { -1, -1, -1, },
                { -1,  8, -1, },
                { -1, -1, -1, },
            };

            edgeFilter.Execute(parameter);

            IImage edgeImage = parameter.Image;

            IImage outImage = new Image(originalImage.Width, originalImage.Height);

            IImage grayScaleImage = new Image(originalImage.Width, originalImage.Height);
            

            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    IRGBColor color = originalImage.Pixels[x, y].Color;

                    float r = originalImage.Pixels[x, y].Color.Red * (1 - edgeImage.Pixels[x, y].Ambient.Red);
                    float g = originalImage.Pixels[x, y].Color.Green * (1 - edgeImage.Pixels[x, y].Ambient.Green);
                    float b = originalImage.Pixels[x, y].Color.Blue * (1 - edgeImage.Pixels[x, y].Ambient.Blue);

                    outImage.Pixels[x, y] = new PixelColor()
                    {
                        Color = new RGBColor(r, g, b)
                    };
                }
            }

            //outImage.GetBitmap(ColorType.Final).Save("out.png", ImageFormat.Png);
            parameter.Image = outImage;
        }

        public void Parse(Parameters parameters)
        {
            
        }
    }
}
