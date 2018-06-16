using RayRender.Interfaces;
using RayRender.Materials;
using RayRender.Utils;

namespace RayRender.Shapes
{
    public abstract class PrimitiveShape : IPrimitiveShape
    {
        public IMaterial Material { get; set; }

        public IVector Position { get; set; }

        public PrimitiveShape(IMaterial material, IVector position)
        {
            this.Material = material;
            this.Position = position;
        }

        public abstract IRayHit Intersect(IRay ray);

        public virtual void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "position":
                    this.Position = parameters.GetVector(1);
                    break;
                case "material":
                    switch (parameters.GetString(1))
                    {
                        case "solid":
                            this.Material = new SolidColorMaterial();
                            break;
                    }
                    break;
            }

            if (this.Material != null)
            {
                this.Material.Parse(parameters);
            }
        }
    }
}
