using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Rays;
using RayRender.Utils;

namespace RayRender.Shapes
{
    public class Plane : PrimitiveShape
    {
        public IVector Normal { get; set; }

        public Plane(IMaterial material, IVector position, IVector normal) : base(material, position)
        {
            this.Normal = normal;
        }

        public Plane() : this(null, null, null)
        {

        }

        public override IRayHit Intersect(IRay ray)
        {
            float denominator = this.Normal.Dot(ray.Direction);

            if (Math.Abs(denominator) > 0.0f)
            {
                IVector difference = this.Position.Minus(ray.Origin);

                float t = difference.Dot(this.Normal) / denominator;

                if (t > 0.0f)
                {
                    IVector intersectionPoint = ray.GetEndAt(t);

                    return new RayHit(ray, this, this, this.Normal, intersectionPoint, t, false);
                }
            }

            return null;
        }

        public override void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "normal":
                    this.Normal = parameters.GetVector(1);
                    break;
            }
            base.Parse(parameters);
        }

        public override string ToString()
        {
            return string.Format("Center {0}; Normal {1}", this.Position, this.Normal);
        }
    }
}
