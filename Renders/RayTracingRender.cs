using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Rays;
using RayRender.Shadings;
using RayRender.Utils;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace RayRender.Renders
{
    

    public class RayTracingRender : IRayRender
    {
        public int MaxRecursionLevel { get; set; }

        public bool AntiAliasing { get; set; }

        public IShading Shading { get; set; }

        public IPixelColor BackGroundColor { get; set; }

        public IWorld World { get; set; }

        public IShading AuxiliarShading { get; set; }

        public Image AuxiliarImage { get; set; }

        public RayTracingRender()
        {
            this.MaxRecursionLevel = 5;
            this.BackGroundColor = new PixelColor
            {
                Ambient = new RGBColor(0.5f, 0.5f, 0.5f),
                Diffuse = new RGBColor(0.0f, 0.0f, 0.0f),
                Specular = new RGBColor(0.0f, 0.0f, 0.0f),
                Color = new RGBColor(0.5f, 0.5f, 0.5f)
            };
            this.World = null;
            this.Shading = null;
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

        public IPixelColor Trace(IRay ray, int depth)
        {
            IRayHit hit = this.FindHit(ray);

            if (hit == null)
            {
                return this.BackGroundColor;
            }

            IPixelColor color = new PixelColor
            {
                Ambient = new RGBColor(0.0f, 0.0f, 0.0f),
                Diffuse = new RGBColor(0.0f, 0.0f, 0.0f),
                Specular = new RGBColor(0.0f, 0.0f, 0.0f),
                Color = new RGBColor(0.0f, 0.0f, 0.0f)
            };

            this.World.Lights.ForEach(delegate (ILight light)
            {
                IPixelColor lightColor = this.Shading.GetColor(hit, light);

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

        public IPixelColor GetPixelColor(int col, int row)
        {
            int bmpRow = this.World.Camera.Height - 1 - row;

            if (this.AntiAliasing)
            {
                IRay ray = this.World.Camera.GetRay(col, bmpRow, 0, 0);
                IPixelColor c1 = this.Trace(ray, 0);
                ray = this.World.Camera.GetRay(col, bmpRow, 0.5f, 0);
                IPixelColor c2 = this.Trace(ray, 0);
                ray = this.World.Camera.GetRay(col, bmpRow, 0, 0.5f);
                IPixelColor c3 = this.Trace(ray, 0);
                ray = this.World.Camera.GetRay(col, bmpRow, 0.5f, 0.5f);
                IPixelColor c4 = this.Trace(ray, 0);

                IPixelColor color = PixelColor.Avg(c1, c2, c3, c4);

                return color;
            }
            else
            {
                IRay ray = this.World.Camera.GetRay(col, bmpRow);
                IPixelColor color = this.Trace(ray, 0);

                return color;
            }
        }

        public void Execute(IWorld parameter)
        {
            Stopwatch watch = Stopwatch.StartNew();

            Logger.Debug("Rendering in RayTracing with {0} levels of recursion", this.MaxRecursionLevel);

            this.World = parameter;

            this.AuxiliarShading = new AmbientShading();
            this.AuxiliarImage = new Image(this.World.Camera.Width, this.World.Camera.Height);

            int maxPixels = this.World.Camera.Width * this.World.Camera.Height;

            IPixelColor[,] pixels = new IPixelColor[this.World.Camera.Width, this.World.Camera.Height];

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
                    this.World.Image.Pixels[c, r] = pixels[c, r];
                }
            }

            //this.World.Image.GetBitmap(ColorType.Ambient).Save("ambient.png", ImageFormat.Png);

            watch.Stop();

            Logger.Debug("Rendered in {0} milliseconds", watch.ElapsedMilliseconds);
        }

        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "background":
                    IRGBColor color = parameters.GetColor(1);
                    this.BackGroundColor = new PixelColor
                    {
                        Ambient = color,
                        Diffuse = new RGBColor(0.0f, 0.0f, 0.0f),
                        Specular = new RGBColor(0.0f, 0.0f, 0.0f),
                        Color = color
                    };
                    break;
                case "level":
                    this.MaxRecursionLevel = parameters.GetInt(1);
                    break;
                case "antialiasing":
                    this.AntiAliasing = parameters.GetBool(1);
                    break;
                case "shading":
                    this.Shading = ShadingFactory.Create(parameters.GetString(1));
                    break;
            }
        }
    }
}
