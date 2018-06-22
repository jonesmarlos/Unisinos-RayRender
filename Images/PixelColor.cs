using RayRender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Images
{
    public class PixelColor : IPixelColor
    {
        public IRGBColor Ambient { get; set; }
        public IRGBColor Diffuse { get; set; }
        public IRGBColor Specular { get; set; }
        public IRGBColor Color { get; set; }

        public IPixelColor Blend(IPixelColor other)
        {
            IPixelColor result = new PixelColor
            {
                Ambient = this.Ambient.Blend(other.Ambient),
                Diffuse = this.Diffuse.Blend(other.Diffuse),
                Specular = this.Specular.Blend(other.Specular),
                Color = this.Color.Blend(other.Color)
            };
            return result;
        }

        public IPixelColor Intensify(IPixelColor other)
        {
            IPixelColor result = new PixelColor
            {
                Ambient = this.Ambient.Intensify(other.Ambient),
                Diffuse = this.Diffuse.Intensify(other.Diffuse),
                Specular = this.Specular.Intensify(other.Specular),
                Color = this.Color.Intensify(other.Color)
            };
            return result;
        }

        public IPixelColor Intensify(float scalar)
        {
            IPixelColor result = new PixelColor
            {
                Ambient = this.Ambient.Intensify(scalar),
                Diffuse = this.Diffuse.Intensify(scalar),
                Specular = this.Specular.Intensify(scalar),
                Color = this.Color.Intensify(scalar)
            };
            return result;
        }

        public static IPixelColor Avg(params IPixelColor[] colors)
        {
            float factor = 1.0f / colors.Length;

            IRGBColor ambient = new RGBColor(0.0f, 0.0f, 0.0f);
            IRGBColor diffuse = new RGBColor(0.0f, 0.0f, 0.0f);
            IRGBColor specular = new RGBColor(0.0f, 0.0f, 0.0f);
            IRGBColor finalColor = new RGBColor(0.0f, 0.0f, 0.0f);

            foreach (IRGBColor color in colors)
            {
                ambient = ambient.Blend(color.Intensify(factor));
                diffuse = diffuse.Blend(color.Intensify(factor));
                specular = specular.Blend(color.Intensify(factor));
                finalColor = finalColor.Blend(color.Intensify(factor));
            }

            IPixelColor resultColor = new PixelColor
            {
                Ambient = ambient,
                Diffuse = diffuse,
                Specular = specular,
                Color = finalColor
            };

            return resultColor;
        }
    }
}
