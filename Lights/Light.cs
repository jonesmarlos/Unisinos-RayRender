using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;
using System;

namespace RayRender.Lights
{
    public class Light : ILight
    {
        public IVector Position { get; set; }

        public IColor Color { get; set; }

        public Light(IVector position, IColor color)
        {
            this.Position = position;
            this.Color = color;
        }

        public Light() : this(null, null)
        {

        }

        public virtual IColor GetColor(IRayHit hit, IRay lightRay)
        {
            float diffuseFactor = Math.Max(0.0f, hit.Normal.Dot(lightRay.Direction));
            IColor diffuseColor = new PixColor(diffuseFactor, diffuseFactor, diffuseFactor);
            IColor diffuseStrength = hit.IntersectShape.Material.Diffuse.Intensify(diffuseColor);

            IVector halfway = lightRay.Direction.Halfway(hit.Ray.Direction.Negate());
            float specularFactor = ((float)Math.Pow(Math.Max(0.0f, hit.Normal.Dot(halfway)), hit.IntersectShape.Material.Shiny));
            IColor specularColor = new PixColor(specularFactor, specularFactor, specularFactor);
            IColor specularStrength = hit.IntersectShape.Material.Specular.Intensify(specularColor);

            IColor shapeColor = hit.IntersectShape.Material.GetColor(hit.Intersection);

            float r = this.Color.Red * (shapeColor.Red * diffuseStrength.Red + specularStrength.Red);
            float g = this.Color.Green * (shapeColor.Green * diffuseStrength.Green + specularStrength.Green);
            float b = this.Color.Blue * (shapeColor.Blue * diffuseStrength.Blue + specularStrength.Blue);

            return new PixColor(r, g, b);
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "position":
                    this.Position = parameters.GetVector(1);
                    break;
                case "color":
                    this.Color = parameters.GetColor(1);
                    break;
            }
        }
    }
}
