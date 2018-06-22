
namespace RayRender.Interfaces
{
    public interface IImage
    {
        IRGBColor[,] Pixels { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        IRGBColor GetColor(int x, int y);

        void SetColor(int x, int y, IRGBColor color);
    }
}
