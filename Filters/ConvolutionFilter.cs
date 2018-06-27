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
    public class ConvolutionFilter : IFilter
    {
        public int Size { get; set; }

        public float Factor { get; set; }

        public float[,] Kernel { get; set; }

        private static int Bound(int value, int endIndex)
        {
            if (value < 0)
                return 0;
            if (value < endIndex)
                return value;
            return endIndex - 1;
        }

        public void Execute(IWorld parameter)
        {
            IImage inputImage = parameter.Image;
            
            //inputImage.GetBitmap(ColorType.AmbientGrayScale).Save("ambient_before.png", ImageFormat.Png);

            int inputWidth = inputImage.Width;
            int inputHeight = inputImage.Height;

            IImage outputImage = new Image(inputWidth, inputHeight);

            int kernelWidth = this.Size;
            int kernelHeight = this.Size;

            int kernelWidthRadius = (kernelWidth - 1) / 2;
            int kernelHeightRadius = (kernelHeight - 1)  / 2;

            for (int w = 0; w < inputWidth; w++)
            {
                for (int h = 0; h < inputHeight; h++)
                {
                    float red = 0.0f;
                    float green = 0.0f;
                    float blue = 0.0f;

                    for (int kw = 0; kw < kernelWidth; kw++)
                    {
                        for (int kh = 0; kh < kernelHeight; kh++)
                        {
                            float kernelValue = this.Kernel[kw, kh];

                            int iw = w + kw - kernelWidthRadius;
                            int ih = h + kh - kernelHeightRadius;

                            if (iw >= 0 && iw < inputWidth && ih >= 0 & ih < inputHeight)
                            {
                                IRGBColor color = inputImage.Pixels[iw, ih].Ambient.GetGrayScale();

                                red += color.Red * kernelValue;
                                green += color.Green * kernelValue;
                                blue += color.Blue * kernelValue;
                            }
                        }
                    }

                    red *= this.Factor;
                    if (red > 0.001f)
                    {
                        red = 1;
                    } else
                    {
                        red = 0;
                    }
                    green *= this.Factor;
                    if (green > 0.001f)
                    {
                        green = 1;
                    } else
                    {
                        green = 0;
                    }
                    blue *= this.Factor;
                    if (blue > 0.001f)
                    {
                        blue = 1;
                    } else
                    {
                        blue = 0;
                    }

                    outputImage.Pixels[w, h] = new PixelColor()
                    {
                      Ambient = new RGBColor(red, green, blue)
                    };
                }
            }

            //outputImage.GetBitmap(ColorType.Ambient).Save("ambient_after.png", ImageFormat.Png);

            parameter.Image = outputImage;
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "size":
                    this.Size = parameters.GetInt(1);
                    this.Kernel = new float[this.Size, this.Size];
                    break;
                case "factor":
                    this.Factor = parameters.GetFloat(1);
                    break;
                case "kernel":
                    float[] kernelValues = parameters.GetFloats(1, this.Size * this.Size);
                    for (int i = 0; i < kernelValues.Length; i++)
                    {
                        int x = i % this.Size;
                        int y = i / this.Size;
                        this.Kernel[x, y] = kernelValues[i];
                    }
                    break;
            }
            
        }
    }
}
