
namespace RayRender.Interfaces
{
    public interface ICamera
    {
        IVector Eye { get; set; }

        IVector Direction { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        void SetViewPort(int width, int height);

        IRay GetRay(int x, int y);

        IRay GetRay(int col, int row, float pixelAdjustmentX, float pixelAdjustmentY);
    }
}
