using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Rays;
using System;

namespace RayRender.Shadings
{
    public class PhongShading : AbstractShading
    {
        public PhongShading()
        {
        }

        public override IPixelColor GetPointLightColor(IRayHit hit, ILight light)
        {
            IVector lightRayVec = new Vector(hit.Intersection, light.Position);
            IRay lightRay = new Ray(hit.Intersection, lightRayVec, lightRayVec.Length());

            float diffuseFactor = Math.Max(0.0f, hit.Normal.Dot(lightRay.Direction));
            IRGBColor diffuseColor = new RGBColor(diffuseFactor, diffuseFactor, diffuseFactor);
            IRGBColor diffuseStrength = hit.IntersectShape.Material.Diffuse.Intensify(diffuseColor);

            IVector halfway = lightRay.Direction.Halfway(hit.Ray.Direction.Negate());
            float specularFactor = ((float)Math.Pow(Math.Max(0.0f, hit.Normal.Dot(halfway)), hit.IntersectShape.Material.Shiny));
            IRGBColor specularColor = new RGBColor(specularFactor, specularFactor, specularFactor);
            IRGBColor specularStrength = hit.IntersectShape.Material.Specular.Intensify(specularColor);

            IRGBColor shapeColor = hit.IntersectShape.Material.GetColor(hit.Intersection);

            float r = light.Color.Red * (shapeColor.Red * diffuseStrength.Red + specularStrength.Red);
            float g = light.Color.Green * (shapeColor.Green * diffuseStrength.Green + specularStrength.Green);
            float b = light.Color.Blue * (shapeColor.Blue * diffuseStrength.Blue + specularStrength.Blue);

            IPixelColor pixelLightColor = new PixelColor
            {
                Ambient = new RGBColor(0.0f, 0.0f, 0.0f),
                Diffuse = new RGBColor(diffuseStrength.Red, diffuseStrength.Green, diffuseStrength.Blue),
                Specular = new RGBColor(specularStrength.Red, specularStrength.Green, specularStrength.Blue),
                Color = new RGBColor(r, g, b)
            };
            
            return pixelLightColor;
        }
    }
}
