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
    public class HDRFilter : IFilter
    {
        public float Alpha { get; set; }
        public int ZoneWidth { get; set; }
        public int ZoneHeight { get; set; }

        public void Execute(IWorld parameter)
        {
            IImage image = parameter.Image;

            //float[,] intensityMatrix = new float[image.Width, image.Height];

            int widthZoneCount = image.Width / this.ZoneWidth;
            int heightZoneCount = image.Height / this.ZoneHeight;
            int sizeZone = this.ZoneWidth * this.ZoneHeight;
            float factorZone = 1.0f / sizeZone;

            float[,] intensityAverage = new float[widthZoneCount, heightZoneCount];

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    IRGBColor color = image.GetColor(x, y);

                    int ix = x / this.ZoneWidth;
                    int iy = y / this.ZoneHeight;

                    intensityAverage[ix, iy] += (color.Red * 0.27f + color.Green * 0.66f + color.Blue * 0.06f) * factorZone;
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    int ix = x / this.ZoneWidth;
                    int iy = y / this.ZoneHeight;

                    float intensity = this.Alpha / intensityAverage[ix, iy];

                    image.Pixels[x, y].Color = image.Pixels[x, y].Color.Blend(new RGBColor(intensity, intensity, intensity));
                }
            }
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "alpha":
                    this.Alpha = parameters.GetFloat(1);
                    break;
                case "width":
                    this.ZoneWidth = parameters.GetInt(1);
                    break;
                case "height":
                    this.ZoneHeight = parameters.GetInt(1);
                    break;
            }
        }
    }
}
