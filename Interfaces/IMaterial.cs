
namespace RayRender.Interfaces
{
    public interface IMaterial : IParseable
    {
        IColor Ambient { get; set; }

        IColor Diffuse { get; set; }

        IColor Specular { get; set; }

        float Shiny { get; set; }

        float Reflection { get; set; }

        float Refraction { get; set; }

        float IndexOfRefraction { get; set; }

        bool IsReflective();

        bool IsRefractive();

        IColor GetColor(IVector point);
    }
}
