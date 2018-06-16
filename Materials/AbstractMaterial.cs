using RayRender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Materials
{
    public abstract class AbstractMaterial
    {
        public float Ambient { get; set; }

        public float Diffuse { get; set; }

        public float Specular { get; set; }

        public float Shiny { get; set; }

        public float Reflection { get; set; }

        public float Refraction { get; set; }

        public float IOR { get; set; }

        public AbstractMaterial(float ambient, float diffuse, float specular, float shiny, float reflection, float refraction, float ior)
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

        public abstract PixColor GetColor(Vector point);

    }
}
