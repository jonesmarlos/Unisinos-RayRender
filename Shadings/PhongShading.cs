using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Rays;
using RayRender.Utils;
using System;

namespace RayRender.Shadings
{
    public class PhongShading : IShading
    {
        public PhongShading()
        {
        }

        private IColor GetAmbientLightColor(IRayHit hit, ILight light)
        {
            IColor ambientColor = hit.IntersectShape.Material.Ambient.Intensify(light.Color);

            IColor shapeColor = hit.IntersectShape.Material.GetColor(hit.Intersection);

            IColor lightColor = ambientColor.Intensify(shapeColor);

            return lightColor;
        }

        private IColor GetPointLightColor(IRayHit hit, ILight light)
        {
            IVector lightRayVec = new Vector(hit.Intersection, light.Position);
            IRay lightRay = new Ray(hit.Intersection, lightRayVec, lightRayVec.Length());

            float diffuseFactor = Math.Max(0.0f, hit.Normal.Dot(lightRay.Direction));
            IColor diffuseColor = new PixColor(diffuseFactor, diffuseFactor, diffuseFactor);
            IColor diffuseStrength = hit.IntersectShape.Material.Diffuse.Intensify(diffuseColor);

            IVector halfway = lightRay.Direction.Halfway(hit.Ray.Direction.Negate());
            float specularFactor = ((float)Math.Pow(Math.Max(0.0f, hit.Normal.Dot(halfway)), hit.IntersectShape.Material.Shiny));
            IColor specularColor = new PixColor(specularFactor, specularFactor, specularFactor);
            IColor specularStrength = hit.IntersectShape.Material.Specular.Intensify(specularColor);

            IColor shapeColor = hit.IntersectShape.Material.GetColor(hit.Intersection);

            float r = light.Color.Red * (shapeColor.Red * diffuseStrength.Red + specularStrength.Red);
            float g = light.Color.Green * (shapeColor.Green * diffuseStrength.Green + specularStrength.Green);
            float b = light.Color.Blue * (shapeColor.Blue * diffuseStrength.Blue + specularStrength.Blue);

            IColor lightColor = new PixColor(r, g, b);

            return lightColor;
        }

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
