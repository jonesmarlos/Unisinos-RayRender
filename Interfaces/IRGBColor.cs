
namespace RayRender.Interfaces
{
    public interface IRGBColor
    {
        float Red { get; set; }

        float Green { get; set; }

        float Blue { get; set; }

        IRGBColor Blend(IRGBColor other);

        IRGBColor Intensify(IRGBColor other);

        IRGBColor Intensify(float scalar);

        IRGBColor GetGrayScale();
    }
}
