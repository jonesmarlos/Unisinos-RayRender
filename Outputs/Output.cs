using RayRender.Interfaces;

namespace RayRender.Outputs
{
    public class Output : IOutput
    {
        public IWriter Writer { get; set; }

        public void Execute(IWorld parameter)
        {
            this.Writer.Execute(parameter);
        }
    }
}
