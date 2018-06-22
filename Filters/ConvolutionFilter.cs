using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Filters
{
    public class ConvolutionFilter : IFilter
    {
        public int Size { get; set; }

        public float KernelDivisor { get; set; }

        public float[,] KernelData { get; set; }

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
            IImage image = parameter.Image;

            int inputWidth = image.Width;
            int inputHeight = image.Height;

            int kernelWidth = this.Size;
            int kernelHeight = this.Size;

            int kernelWidthRadius = kernelWidth / 2;
            int kernelHeightRadius = kernelHeight  / 2;

            for (int i = inputWidth - 1; i >= 0; i--)
            {
                for (int j = inputHeight - 1; j >= 0; j--)
                {
                    IRGBColor newColor = new RGBColor(0.0f, 0.0f, 0.0f);
                    for (int kw = kernelWidth - 1; kw >= 0; kw--)
                    {
                        for (int kh = kernelHeight - 1; kh >= 0; kh--)
                        {
                            float kv = this.KernelData[kw, kh];

                            IRGBColor color = image.GetColor(Bound(i + kw - kernelWidthRadius, inputWidth), Bound(j + kh - kernelHeightRadius, inputHeight));

                            color = color.Intensify(kv);

                            newColor = newColor.Blend(color);
                        }
                    }

                    newColor = newColor.Intensify(1.0f / this.KernelDivisor);

                    image.SetColor(i, j, newColor);
                }
            }
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "size":
                    this.Size = parameters.GetInt(1);
                    this.KernelData = new float[this.Size, this.Size];
                    break;
                case "divisor":
                    this.KernelDivisor = parameters.GetFloat(1);
                    break;
                case "kernel":
                    float[] kernelValues = parameters.GetFloats(1);
                    for (int i = 0; i < kernelValues.Length; i++)
                    {
                        int x = i % this.Size;
                        int y = i / this.Size;
                        this.KernelData[x, y] = kernelValues[i];
                    }
                    break;
            }
            
        }
    }
}
