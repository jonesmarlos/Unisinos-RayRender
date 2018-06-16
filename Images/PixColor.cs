using RayRender.Interfaces;
using System;

namespace RayRender.Images
{
    public class PixColor : IColor
    {
        private float red;

        public float Red
        {
            get => this.red;
            set => this.red = PixColor.EnsureChannel(value);
        }

        private float green;

        public float Green
        {
            get => this.green;
            set => this.green = PixColor.EnsureChannel(value);
        }

        private float blue;

        public float Blue
        {
            get => this.blue;
            set => this.blue = PixColor.EnsureChannel(value);
        }

        public PixColor(float red, float green, float blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public IColor Blend(IColor other)
        {
            float r = this.Red + other.Red;
            float g = this.Green + other.Green;
            float b = this.Blue + other.Blue;

            return new PixColor(r, g, b);
        }

        public IColor Intensify(IColor other)
        {
            float r = this.Red * other.Red;
            float g = this.Green * other.Green;
            float b = this.Blue * other.Blue;

            return new PixColor(r, g, b);
        }

        public IColor Intensify(float scalar)
        {
            IColor color = new PixColor(scalar, scalar, scalar);
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

        public static IColor Avg(params IColor[] colors)
        {
            float factor = 1.0f / colors.Length;

            IColor resultColor = new PixColor(0, 0, 0);

            foreach(IColor color in colors)
            {
                resultColor.Red += color.Red * factor;
                resultColor.Green += color.Green * factor;
                resultColor.Blue += color.Blue * factor;
            }

            return resultColor;
        }
    }
}
