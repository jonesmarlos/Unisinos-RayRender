using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;
using System;
using System.Collections.Generic;

namespace RayRender.Filters
{
    public class Blur : IFilter
    {
        public int Level { get; set; }

        public void Execute(IWorld parameter)
        {
            Logger.Debug("Filter Blur");

            IImage image = parameter.Image;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    List<IColor> colors = new List<IColor>();

                    for (int ix = x - this.Level; ix < x + this.Level; ix++)
                    {
                        for (int iy = y - this.Level; iy < y + this.Level; iy++)
                        {
                            if (ix >= 0 && ix < image.Width && iy >= 0 && iy < image.Height)
                            {
                                IColor color = image.GetColor(ix, iy);
                                colors.Add(color);
                            }
                        }
                    }

                    IColor avgColor = PixColor.Avg(colors.ToArray());
                    
                    image.SetColor(x, y, avgColor);
                }
            }
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "level":
                    this.Level = parameters.GetInt(1);
                    break;
            }
        }
    }
}
