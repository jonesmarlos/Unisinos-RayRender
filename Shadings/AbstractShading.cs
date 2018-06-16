using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Shadings
{
    public abstract class AbstractShading : IShading
    {

        public IColor GetAmbientLightColor(IRayHit hit, ILight light)
        {
            IColor ambientColor = hit.IntersectShape.Material.Ambient.Intensify(light.Color);

            IColor shapeColor = hit.IntersectShape.Material.GetColor(hit.Intersection);

            IColor lightColor = ambientColor.Intensify(shapeColor);

            return lightColor;
        }

        public abstract IColor GetPointLightColor(IRayHit hit, ILight light);

        public IColor GetColor(IRayHit hit, ILight light)
        {
            switch (light.Type)
            {
                case LightType.AmbientLight:
                    return this.GetAmbientLightColor(hit, light);
                case LightType.PointLight:
                    return this.GetPointLightColor(hit, light);
                default:
                    return new PixColor(0.0f, 0.0f, 0.0f);
            }
        }

        public void Parse(Parameters parameters)
        {

        }
    }
}
