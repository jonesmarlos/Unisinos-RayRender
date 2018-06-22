using RayRender.Interfaces;
using System;

namespace RayRender.Images
{
    public class RGBColor : IRGBColor
    {
        private float red;

        public float Red
        {
            get => this.red;
            set => this.red = RGBColor.EnsureChannel(value);
        }

        private float green;

        public float Green
        {
            get => this.green;
            set => this.green = RGBColor.EnsureChannel(value);
        }

        private float blue;

        public float Blue
        {
            get => this.blue;
            set => this.blue = RGBColor.EnsureChannel(value);
        }

        public RGBColor(float red, float green, float blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public IRGBColor Blend(IRGBColor other)
        {
            float r = this.Red + other.Red;
            float g = this.Green + other.Green;
            float b = this.Blue + other.Blue;

            return new RGBColor(r, g, b);
        }

        public IRGBColor Intensify(IRGBColor other)
        {
            float r = this.Red * other.Red;
            float g = this.Green * other.Green;
            float b = this.Blue * other.Blue;

            return new RGBColor(r, g, b);
        }

        public IRGBColor Intensify(float scalar)
        {
            IRGBColor color = new RGBColor(scalar, scalar, scalar);
            return this.Intensify(color);
        }

        public override string ToString()
        {
            return string.Format("Color([{0:F18}, {1:F18}, {2:F18}])", this.Red, this.Green, this.Blue);
        }

        private static float EnsureChannel(float value)
        {
            return Math.Max(0.0f, Math.Min(1.0f, value));
        }

        public IRGBColor GetGrayScale()
        {
            float grayScale = this.Red * 0.27f + this.Green * 0.67f + this.Blue * 0.06f;

            return new RGBColor(grayScale, grayScale, grayScale);
        }
        public static IRGBColor Avg(params IRGBColor[] colors)
        {
            float factor = 1.0f / colors.Length;

            IRGBColor resultColor = new RGBColor(0, 0, 0);

            foreach(IRGBColor color in colors)
            {
                resultColor.Red += color.Red * factor;
                resultColor.Green += color.Green * factor;
                resultColor.Blue += color.Blue * factor;
            }

            return resultColor;
        }

    }
}
