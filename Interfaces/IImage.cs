
using System.Drawing;

namespace RayRender.Interfaces
{
    public enum ColorType
    {
        Ambient,
        Diffuse,
        Specular,
        Final,
        AmbientGrayScale,
        DiffuseGrayScale,
        SpecularGrayScale,
        FinalGrayScale
    }

    public interface IImage
    {
        IPixelColor[,] Pixels { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        IRGBColor GetColor(int x, int y);

        void SetColor(int x, int y, IRGBColor color);

        Bitmap GetBitmap(ColorType colorType);
    }
}
