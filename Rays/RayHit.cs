using RayRender.Interfaces;
using System;

namespace RayRender.Rays
{
    public class RayHit : IRayHit
    {
        public IRay Ray { get; set; }

        public IShape MasterShape { get; set; }

        public IPrimitiveShape IntersectShape { get; set; }

        public IVector Normal { get; set; }

        public IVector Intersection { get; set; }

        public float Time { get; set; }

        public bool InComing { get; set; }

        public RayHit(IRay ray, IShape masterShape, IPrimitiveShape intersectShape, IVector normal, IVector intersection, float time, bool incoming)
        {
            this.Ray = ray;
            this.MasterShape = masterShape;
            this.IntersectShape = intersectShape;
            this.Normal = normal.Normalize();
            this.Intersection = intersection;
            this.Time = time;
            this.InComing = incoming;
        }

        public IRay GetReflectionRay()
        {
            float cos = this.Ray.Direction.Dot(this.Normal);
            IVector times = this.Normal.Times(2.0f * cos);
            IVector rayDirection = this.Ray.Direction.Minus(times);

            return new Ray(this.Intersection, rayDirection);
        }

        public IRay GetRefractionRay()
        {
            IVector v = this.Ray.Direction.Negate();
            IVector n = this.Normal;
            float cosi = v.Dot(n);
            float nint;
            if (this.InComing)
            {
                nint = 1.0f / this.IntersectShape.Material.IndexOfRefraction;
            }
            else
            {
                nint = this.IntersectShape.Material.IndexOfRefraction;
            }
            float cost = (float)Math.Sqrt(1.0f - nint * nint * (1 - cosi * cosi));

            return new Ray(this.Intersection, n.Times(nint * cosi - cost).Minus(v.Times(nint)));
        }
    }
}
