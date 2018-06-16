using RayRender.Interfaces;

namespace RayRender.Stages
{
    public class PreProcessing : IPreProcessing
    {
        public void Execute(IWorld parameter)
        {
            int width = parameter.Image.Width;
            int height = parameter.Image.Height;
            parameter.Camera.SetViewPort(width, height);
        }
    }
}
