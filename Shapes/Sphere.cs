using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Rays;
using RayRender.Utils;
using System;

namespace RayRender.Shapes
{
    public class Sphere : PrimitiveShape
    {
        public IVector Radius { get; set; }

        public Sphere(IMaterial material, IVector position, IVector radius) : base(material, position)
        {
            this.Radius = radius;
        }

        public Sphere() : this(null, null, null)
        {

        }

        public override IRayHit Intersect(IRay ray)
        {
            IVector p = ray.Origin;
            IVector u = ray.Direction;
            IVector v = new Vector(this.Position, p);
            float b = 2 * (v.Dot(u));
            float c = v.Dot(v) - this.Radius.X * this.Radius.X;
            float discriminant = b * b - 4 * c;

            if (discriminant < 0)
            {
                return null;
            }

            float timeMinus = (b * -1 - ((float)Math.Sqrt(discriminant))) / 2;
            float timePlus = (b * -1 + ((float)Math.Sqrt(discriminant))) / 2;

            if (timeMinus < 0 && timePlus < 0)
            {
                return null;
            }

            IVector intersection;
            IVector normal;
            float timeIntersection;
            bool incoming;

            if (timeMinus < 0 && timePlus > 0)
            {
                timeIntersection = timePlus;
                intersection = ray.GetEndAt(timePlus);
                normal = new Vector(intersection, this.Position);
                incoming = false;
            }
            else
            {
                timeIntersection = timeMinus;
                intersection = ray.GetEndAt(timeMinus);
                normal = new Vector(this.Position, intersection);
                incoming = true;
            }

            return new RayHit(ray, this, this, normal.Normalize(), intersection, timeIntersection, incoming);
        }

        public override void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "radius":
                    this.Radius = parameters.GetVector(1);
                    break;
            }

            base.Parse(parameters);
        }
    }
}
