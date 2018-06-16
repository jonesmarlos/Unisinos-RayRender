
namespace RayRender.Interfaces
{
    public interface IRay
    {
        IVector Origin { get; set; }

        IVector Direction { get; set; }

        float Time { get; set; }

        IVector GetEndAt(float time);
    }
}
