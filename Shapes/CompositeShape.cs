using RayRender.Interfaces;
using RayRender.Utils;
using System.Collections.Generic;

namespace RayRender.Shapes
{
    public class CompositeShape : IShape
    {
        public List<IShape> Shapes;

        public CompositeShape()
        {
            this.Shapes = new List<IShape>();
        }

        public IRayHit Intersect(IRay ray)
        {
            IRayHit rayHit = null;
            float time = float.MaxValue;

            foreach (IShape shape in this.Shapes)
            {
                IRayHit h = shape.Intersect(ray);
                if (h != null && h.Time < time)
                {
                    rayHit = h;
                    time = h.Time;
                }
            }

            return rayHit;
        }

        public void Parse(Parameters parameters)
        {
            this.Shapes.ForEach(delegate (IShape shape)
            {
                shape.Parse(parameters);
            });
        }
    }
}
