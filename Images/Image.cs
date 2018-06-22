using RayRender.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace RayRender.Images
{
    public class Image : IImage
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public IRGBColor[,] Pixels { get; set; }

        public Image(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Pixels = new IRGBColor[width, height];
        }

        public Image() : this(0, 0)
        {

        }

        public Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);

            for (int w = 0; w < this.Width; w++)
            {
                for (int h = 0; h < this.Height; h++)
                {
                    float fr = this.Pixels[w, h].Red * Byte.MaxValue;
                    float fg = this.Pixels[w, h].Green * Byte.MaxValue;
                    float fb = this.Pixels[w, h].Blue * Byte.MaxValue;

                    byte br = (byte)fr;
                    byte bg = (byte)fg;
                    byte bb = (byte)fb;

                    Color color = Color.FromArgb(255, br, bg, bb);

                    bitmap.SetPixel(w, h, color);

                }
            }

            return bitmap;
        }

        public IRGBColor GetColor(int x, int y)
        {
            return this.Pixels[x, y];
        }

        public void SetColor(int x, int y, IRGBColor pixColor)
        {
            this.Pixels[x, y] = pixColor;
        }
    }
}
