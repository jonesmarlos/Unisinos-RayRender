using RayRender.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace RayRender.Images
{
    public class PixImage : IImage
    {
        private Bitmap bitmapImage;

        public int Width
        {
            get => this.bitmapImage.Width;
            set => throw new NotImplementedException();
        }

        public int Height
        {
            get => this.bitmapImage.Height;
            set => throw new NotImplementedException();
        }

        public PixImage(int width, int height)
        {
            this.bitmapImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        public PixImage() : this(0, 0)
        {

        }

        public Bitmap GetBitmap()
        {
            return this.bitmapImage;
        }

        public IColor GetColor(int x, int y)
        {
            Color color = this.bitmapImage.GetPixel(x, y);

            return new PixColor((color.R * 1.0f / Byte.MaxValue), (color.G * 1.0f / Byte.MaxValue), (color.B * 1.0f / Byte.MaxValue));
        }

        public void SetColor(int x, int y, IColor pixColor)
        {
            float fr = pixColor.Red * Byte.MaxValue;
            float fg = pixColor.Green * Byte.MaxValue;
            float fb = pixColor.Blue * Byte.MaxValue;

            byte br = (byte)fr;
            byte bg = (byte)fg;
            byte bb = (byte)fb;

            Color color = Color.FromArgb(255, br, bg, bb);

            this.bitmapImage.SetPixel(x, y, color);
        }
    }
}
