
namespace RayRender.Interfaces
{
    public interface IInput : IStage
    {
        IReader Reader { get; set; }
    }
}
