using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayRender.Images;
using RayRender.Interfaces;

namespace RayRender.Shadings
{
    public class AmbientShading : AbstractShading
    {
        public override IPixelColor GetPointLightColor(IRayHit hit, ILight light)
        {
            return null;
        }
    }
}
