using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Shadings;
using RayRender.Utils;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RayRender.Renders
{
    public class RayTracingRender : IRayRender
    {
        public int MaxRecursionLevel { get; set; }

        public bool AntiAliasing { get; set; }

        public IShading Shading { get; set; }

        public IColor BackGroundColor { get; set; }

        public IWorld World { get; set; }

        public RayTracingRender()
        {
            this.MaxRecursionLevel = 5;
            this.BackGroundColor = new PixColor(0.5f, 0.5f, 0.5f);
            this.World = null;
            this.Shading = new PhongShading();
        }
        
        public IRayHit FindHit(IRay ray)
        {
            IRayHit rayHit = null;
            float rayTimeHit = float.MaxValue;

            this.World.Shapes.ForEach(delegate (IShape shape)
            {
                IRayHit rayShapeHit = shape.Intersect(ray);
                if (rayShapeHit != null && rayShapeHit.Time < rayTimeHit)
                {
                    rayHit = rayShapeHit;
                    rayTimeHit = rayShapeHit.Time;
                }
            });

            return rayHit;
        }

        public IColor Trace(IRay ray, int depth)
        {
            IRayHit hit = this.FindHit(ray);

            if (hit == null)
            {
                return this.BackGroundColor;
            }

            IColor color = new PixColor(0, 0, 0);

            this.World.Lights.ForEach(delegate (ILight light)
            {
                IColor lightColor = this.Shading.GetColor(hit, light);

                color = color.Blend(lightColor);
            });

            if (depth < this.MaxRecursionLevel)
            {
                if (hit.IntersectShape.Material.IsReflective())
                {
                    color = color.Blend(this.Trace(hit.GetReflectionRay(), depth + 1).Intensify(hit.IntersectShape.Material.Reflection));
                }

                if (hit.IntersectShape.Material.IsRefractive())
                {
                    color = color.Blend(this.Trace(hit.GetRefractionRay(), depth + 1).Intensify(hit.IntersectShape.Material.Refraction));
                }
            }

            return color;
        }

        public IColor GetPixelColor(int col, int row)
        {
            int bmpRow = this.World.Camera.Height - 1 - row;

            if (this.AntiAliasing)
            {
                IRay ray = this.World.Camera.GetRay(col, bmpRow, 0, 0);
                IColor c1 = this.Trace(ray, 0);
                ray = this.World.Camera.GetRay(col, bmpRow, 0.5f, 0);
                IColor c2 = this.Trace(ray, 0);
                ray = this.World.Camera.GetRay(col, bmpRow, 0, 0.5f);
                IColor c3 = this.Trace(ray, 0);
                ray = this.World.Camera.GetRay(col, bmpRow, 0.5f, 0.5f);
                IColor c4 = this.Trace(ray, 0);

                IColor color = PixColor.Avg(c1, c2, c3, c4);

                return color;
            }
            else
            {

                IRay ray = this.World.Camera.GetRay(col, bmpRow);
                IColor color = this.Trace(ray, 0);

                return color;
            }
        }

        public void Execute(IWorld parameter)
        {
            Stopwatch watch = Stopwatch.StartNew();

            Logger.Debug("Rendering in RayTracing with {0} levels of recursion", this.MaxRecursionLevel);

            this.World = parameter;

            int maxPixels = this.World.Camera.Width * this.World.Camera.Height;

            IColor[,] pixels = new IColor[this.World.Camera.Width, this.World.Camera.Height];

            Parallel.For(0, maxPixels,
                index =>
                {
                    int r = index / this.World.Camera.Width;
                    int c = index % this.World.Camera.Width;

                    pixels[c, r] = this.GetPixelColor(c, r);
                });

            for (int r = 0; r < this.World.Camera.Height; r++)
            {
                for (int c = 0; c < this.World.Camera.Width; c++)
                {
                    IColor pixelColor = pixels[c, r];
                    this.World.Image.SetColor(c, r, pixelColor);
                }
            }

            watch.Stop();

            Logger.Debug("Rendered in {0} milliseconds", watch.ElapsedMilliseconds);
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "background":
                    this.BackGroundColor = parameters.GetColor(1);
                    break;
                case "level":
                    this.MaxRecursionLevel = parameters.GetInt(1);
                    break;
                case "antialiasing":
                    this.AntiAliasing = parameters.GetBool(1);
                    break;
            }
        }
    }
}
