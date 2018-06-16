using RayRender.Interfaces;

namespace RayRender.Renders
{
    public class Render : IRender
    {
        public IRayRender RayRender { get; set; }

        public void Execute(IWorld parameter)
        {
            this.RayRender.Execute(parameter);
        }
    }
}
