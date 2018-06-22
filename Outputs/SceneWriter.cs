using RayRender.Images;
using RayRender.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace RayRender.Outputs
{
    public class SceneWriter : IWriter
    {
        public string FileName { get; set; }

        public SceneWriter(string fileName)
        {
            this.FileName = fileName;
        }

        public SceneWriter() : this(string.Empty)
        {

        }

        public void Execute(IWorld parameter)
        {
            Images.Image image = (Images.Image)parameter.Image;
            Bitmap bitmap = image.GetBitmap();
            bitmap.Save(this.FileName, ImageFormat.Png);
        }
    }
}
