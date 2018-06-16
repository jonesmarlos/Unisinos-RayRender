using RayRender.Interfaces;
using RayRender.Utils;
using System;

namespace RayRender.Maths
{
    public class Vector : IVector
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector(IVector other) : this(other.X, other.Y, other.Z)
        {

        }

        public Vector(IVector origin, IVector target) : this(target.X - origin.X, target.Y - origin.Y, target.Z - origin.Z)
        {

        }

        public float Length()
        {
            return (float)Math.Sqrt(this.Dot(this));
        }

        public float DistanceTo(IVector other)
        {
            float x = other.X - this.X;
            float y = other.Y - this.Y;
            float z = other.Z - this.Z;

            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public float Dot(IVector other)
        {
            return (this.X * other.X) + (this.Y * other.Y) + (this.Z * other.Z);
        }

        public IVector Cross(IVector other)
        {
            return new Vector(((this.Y * other.Z) - (this.Z * other.Y)), ((this.Z * other.X) - (this.X * other.Z)), ((this.X * other.Y) - (this.Y * other.X)));
        }

        public IVector Halfway(IVector other)
        {
            return this.Plus(other).Normalize();
        }

        public IVector Minus(IVector other)
        {
            return new Vector(this.X - other.X, this.Y - other.Y, this.Z - other.Z);
        }

        public IVector Negate()
        {
            return this.Times(-1);
        }

        public IVector Normalize()
        {
            float magnitude = this.Length();
            float divisor;
            if (magnitude == 0)
            {
                Logger.Error("Trying to normalize a Vector with length 0.");
                divisor = float.MaxValue;
            }
            else
            {
                divisor = 1 / magnitude;
            }

            return this.Times(divisor);
        }

        public IVector Plus(IVector other)
        {
            return new Vector(this.X + other.X, this.Y + other.Y, this.Z + other.Z);
        }

        public IVector Times(float scalar)
        {
            return new Vector(this.X * scalar, this.Y * scalar, this.Z * scalar);
        }

        public override string ToString()
        {
            return string.Format("[{0:F4}, {1:F4}, {2:F4}]", this.X, this.Y, this.Z);
        }
    }
}
