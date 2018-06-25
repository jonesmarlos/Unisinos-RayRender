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

                    // final[x][y]c = img[x][y]c * (1 - aux[x][y]) + cor_borda * aux[x][y]

                    IRGBColor color = originalImage.Pixels[x, y].Color;

                    float s = (originalImage.Pixels[x, y].Ambient.Red + originalImage.Pixels[x, y].Ambient.Green + originalImage.Pixels[x, y].Ambient.Blue) / 3.0f;

                    float r = color.Red * (1 - s) + edgeImage.Pixels[x, y].Ambient.Red * s;
                    float g = color.Green * (1 - s) + edgeImage.Pixels[x, y].Ambient.Green * s;
                    float b = color.Blue * (1 - s) + edgeImage.Pixels[x, y].Ambient.Blue * s;

                    outImage.Pixels[x, y] = new PixelColor()
                    {
                        Color = new RGBColor(r, g, b)
                    };
                }
            }

            outImage.GetBitmap(ColorType.Final).Save("out.png", ImageFormat.Png);

        }

        public void Parse(Parameters parameters)
        {
            
        }
    }
}
