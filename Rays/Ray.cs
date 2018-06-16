using RayRender.Interfaces;
using System;

namespace RayRender.Rays
{
    public class Ray : IRay
    {
        public IVector Origin { get; set; }

        public IVector Direction { get; set; }

        public float Time { get; set; }

        public Ray(IVector origin, IVector direction, float time)
        {
            this.Time = time;
            this.Direction = direction.Normalize();
            this.Origin = origin.Plus(this.Direction.Times(0.001f));
        }

        public Ray(IVector origin, IVector direction) : this(origin, direction, float.MaxValue)
        {
        }

        public IVector GetEndAt(float time)
        {
            return this.Origin.Plus(this.Direction.Times(time));
        }

        public override string ToString()
        {
            return string.Format("Org:{0} Dir:{1} t:{2:F18}", this.Origin, this.Direction, this.Time);
        }
    }
}