using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;

namespace RayRender.Shadings
{
    public abstract class AbstractShading : IShading
    {

        public IPixelColor GetAmbientLightColor(IRayHit hit, ILight light)
        {
            IRGBColor ambientColor = hit.IntersectShape.Material.Ambient.Intensify(light.Color);

            IRGBColor shapeColor = hit.IntersectShape.Material.GetColor(hit.Intersection);

            IPixelColor pixelLightColor = new PixelColor
            {
                Ambient = ambientColor.Intensify(shapeColor),
                Diffuse = new RGBColor(0.0f, 0.0f, 0.0f),
                Specular = new RGBColor(0.0f, 0.0f, 0.0f),
                Color = ambientColor.Intensify(shapeColor)
            };

            return pixelLightColor;
        }

        public abstract IPixelColor GetPointLightColor(IRayHit hit, ILight light);

        public IPixelColor GetColor(IRayHit hit, ILight light)
        {
            switch (light.Type)
            {
                case LightType.AmbientLight:
                    return this.GetAmbientLightColor(hit, light);
                case LightType.PointLight:
                    return this.GetPointLightColor(hit, light);
                default:
                    return null;
            }
        }

        public void Parse(Parameters parameters)
        {

        }
    }
}
