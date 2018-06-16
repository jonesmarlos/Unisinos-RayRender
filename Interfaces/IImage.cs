
namespace RayRender.Interfaces
{
    public interface IImage
    {
        int Width { get; set; }

        int Height { get; set; }

        IColor GetColor(int x, int y);

        void SetColor(int x, int y, IColor color);
    }
}
