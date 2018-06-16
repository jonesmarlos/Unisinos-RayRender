using RayRender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayRender.Shadings
{
    public class ShadingFactory
    {
        private ShadingFactory()
        {

        }

        public static IShading Create(string shadingType)
        {
            switch (shadingType)
            {
                case "phong":
                    return new PhongShading();
                case "toon":
                    return new ToonShading();
            }

            return null;
        }
    }
}
