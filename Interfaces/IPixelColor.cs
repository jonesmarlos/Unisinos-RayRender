namespace RayRender.Interfaces
{
    public interface IPixelColor
    {
        IRGBColor Ambient { get; set; }
        IRGBColor Diffuse { get; set; }
        IRGBColor Specular { get; set; }
        IRGBColor Color { get; set; }

        IPixelColor Blend(IPixelColor other);

        IPixelColor Intensify(IPixelColor other);

        IPixelColor Intensify(float scalar);
    }
}
