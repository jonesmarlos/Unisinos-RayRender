using RayRender.Interfaces;

namespace RayRender.Inputs
{
    public class Input : IInput
    {
        public IReader Reader { get; set; }

        public void Execute(IWorld parameter)
        {
            this.Reader.Execute(parameter);
        }
    }
}
