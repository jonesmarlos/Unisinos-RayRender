using RayRender.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RayRender.Images
{
    public class Image : IImage
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public IPixelColor[,] Pixels { get; set; }

        public Image(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Pixels = new IPixelColor[width, height];
        }

        public Image() : this(0, 0)
        {

        }

        public static IImage LoadFromFile(string file)
        {
            StreamReader streamReader = new StreamReader(file);
            Bitmap bitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();

            IImage image = new Image(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);

                    float fr = color.R * 1.0f / Byte.MaxValue;
                    float fg = color.G * 1.0f / Byte.MaxValue;
                    float fb = color.B * 1.0f / Byte.MaxValue;

                    image.Pixels[x, y] = new PixelColor();
                    image.Pixels[x, y].Ambient = new RGBColor(fr, fg, fb);
                    image.Pixels[x, y].Color = new RGBColor(fr, fg, fb);
                }
            }

            return image;
        }

        public Bitmap GetBitmap(ColorType colorType)
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);

            for (int w = 0; w < this.Width; w++)
            {
                for (int h = 0; h < this.Height; h++)
                {
                    IRGBColor color = null;

                    switch (colorType)
                    {
                        case ColorType.Ambient:
                            color = this.Pixels[w, h].Ambient;
                            break;
                        case ColorType.Diffuse:
                            color = this.Pixels[w, h].Diffuse;
                            break;
                        case ColorType.Specular:
                            color = this.Pixels[w, h].Specular;
                            break;
                        case ColorType.Final:
                            color = this.Pixels[w, h].Color;
                            break;
                        case ColorType.AmbientGrayScale:
                            color = this.Pixels[w, h].Ambient.GetGrayScale();
                            break;
                        case ColorType.DiffuseGrayScale:
                            color = this.Pixels[w, h].Diffuse.GetGrayScale();
                            break;
                        case ColorType.SpecularGrayScale:
                            color = this.Pixels[w, h].Specular.GetGrayScale();
                            break;
                        case ColorType.FinalGrayScale:
                            color = this.Pixels[w, h].Color.GetGrayScale();
                            break;
                    }

                    float fr = color.Red * Byte.MaxValue;
                    float fg = color.Green * Byte.MaxValue;
                    float fb = color.Blue * Byte.MaxValue;

                    byte br = (byte)fr;
                    byte bg = (byte)fg;
                    byte bb = (byte)fb;

                    Color intColor = Color.FromArgb(255, br, bg, bb);

                    bitmap.SetPixel(w, h, intColor);

                }
            }

            return bitmap;
        }

        public IRGBColor GetColor(int x, int y)
        {
            return this.Pixels[x, y].Color;
        }

        public void SetColor(int x, int y, IRGBColor pixColor)
        {
            this.Pixels[x, y].Color = pixColor;
        }
    }
}
