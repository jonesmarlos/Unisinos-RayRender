using RayRender.Interfaces;
using RayRender.Utils;

namespace RayRender.Materials
{
    public class SolidColorMaterial : IMaterial
    {
        public IColor Ambient { get; set; }

        public IColor Diffuse { get; set; }

        public IColor Specular { get; set; }

        public float Shiny { get; set; }

        public float Reflection { get; set; }

        public float Refraction { get; set; }

        public float IndexOfRefraction { get; set; }

        public IColor Color { get; set; }

        public SolidColorMaterial(IColor ambient, IColor diffuse, IColor specular, float shiny, float reflection, float refraction, float indexOfRefraction, IColor color)
        {
            this.Ambient = ambient;
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Shiny = shiny;
            this.Reflection = reflection;
            this.Refraction = refraction;
            this.IndexOfRefraction = indexOfRefraction;
            this.Color = color;
        }

        public SolidColorMaterial() : this(null, null, null, 0, 0, 0, 0, null)
        {

        }

        public bool IsReflective()
        {
            return this.Reflection > 0;
        }

        public bool IsRefractive()
        {
            return this.Refraction > 0;
        }

        public IColor GetColor(IVector point)
        {
            return this.Color;
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "ambient":
                    this.Ambient = parameters.GetColor(1);
                    break;
                case "diffuse":
                    this.Diffuse = parameters.GetColor(1);
                    break;
                case "specular":
                    this.Specular = parameters.GetColor(1);
                    break;
                case "shiny":
                    this.Shiny = parameters.GetFloat(1);
                    break;
                case "reflection":
                    this.Reflection = parameters.GetFloat(1);
                    break;
                case "refraction":
                    this.Refraction = parameters.GetFloat(1);
                    break;
                case "ior":
                    this.IndexOfRefraction = parameters.GetFloat(1);
                    break;
                case "color":
                    this.Color = parameters.GetColor(1);
                    break;
            }
        }
    }
}
