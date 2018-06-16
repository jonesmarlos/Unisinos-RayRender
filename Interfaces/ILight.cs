
namespace RayRender.Interfaces
{
    public interface ILight : IParseable
    {
        IVector Position { get; set; }

        IColor Color { get; set; }

        IColor GetColor(IRayHit hit, IRay lightRay);
    }
}
