using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Rays;
using System;

namespace RayRender.Core
{
    public class Camera : ICamera
    {
        public IVector Eye { get; set; }

        public IVector Direction { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        private IVector vx;

        private IVector vy;

        private IVector vz;

        private float windowDistance;

        private float windowWidth;

        private float windowHeight;

        public Camera(IVector eye, IVector direction)
        {
            this.Eye = eye;
            this.Direction = direction;
            this.Width = 0;
            this.Height = 0;
            this.vx = null;
            this.vy = null;
            this.vz = null;
            this.windowDistance = 0.0f;
            this.windowWidth = 0.0f;
            this.windowHeight = 0.0f;
        }

        public Camera() : this(null, null)
        {

        }

        public void SetViewPort(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            float fovy = 30.0f * ((float)Math.PI / 180.0f);
            float fovx = fovy * this.Width / this.Height;

            IVector at = new Vector(this.Eye, this.Direction);
            IVector up = new Vector(0.0f, 1.0f, 0.0f);

            this.vz = at.Negate().Normalize();
            this.vx = up.Cross(this.vz).Normalize();
            this.vy = this.vz.Cross(this.vx);

            this.windowDistance = 1.0f;
            this.windowHeight = ((float)Math.Sin(fovy / 2.0f)) * this.windowDistance * 2.0f;
            this.windowWidth = ((float)Math.Sin(fovx / 2.0f)) * this.windowDistance * 2.0f;
        }

        private IVector ConvertCoords(IVector p)
        {
            IMatrix rT = new Matrix(new float[4, 4]
                {
                    { this.vx.X, this.vy.X, this.vz.X, 0.0f },
                    { this.vx.Y, this.vy.Y, this.vz.Y, 0.0f },
                    { this.vx.Z, this.vy.Z, this.vz.Z, 0.0f },
                    { 0.0f, 0.0f, 0.0f, 1.0f }
                });

            IMatrix tInv = new Matrix(new float[4, 4]
                {
                    { 1.0f, 0.0f, 0.0f, this.Eye.X },
                    { 0.0f, 1.0f, 0.0f, this.Eye.Y },
                    { 0.0f, 0.0f, 1.0f, this.Eye.Z },
                    { 0.0f, 0.0f, 0.0f, 1.0f }
                });

            IMatrix matrix = tInv.Times(rT);
            IVector vector = matrix.Times(new Vector(p.X, p.Y, p.Z));

            return vector;
        }

        public IRay GetRay(int x, int y)
        {
            return this.GetRay(x, y, 0.5f, 0.5f);
        }

        public IRay GetRay(int col, int row, float pixelAdjustmentX, float pixelAdjustmentY)
        {
            float x = (((float)col + pixelAdjustmentX) / this.Width) * this.windowWidth - (this.windowWidth / 2.0f);
            float y = (((float)row + pixelAdjustmentY) / this.Height) * this.windowHeight - (this.windowHeight / 2.0f);

            IVector v = new Vector(this.Eye, this.ConvertCoords(new Vector(x, y, -this.windowDistance)));

            IRay ray = new Ray(this.Eye, v);

            return ray;
        }
    }
}
