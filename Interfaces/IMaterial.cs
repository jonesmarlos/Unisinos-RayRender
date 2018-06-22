
namespace RayRender.Interfaces
{
    public interface IMaterial : IParseable
    {
        IRGBColor Ambient { get; set; }

        IRGBColor Diffuse { get; set; }

        IRGBColor Specular { get; set; }

        float Shiny { get; set; }

        float Reflection { get; set; }

        float Refraction { get; set; }

        float IndexOfRefraction { get; set; }

        bool IsReflective();

        bool IsRefractive();

        IRGBColor GetColor(IVector point);
    }
}
