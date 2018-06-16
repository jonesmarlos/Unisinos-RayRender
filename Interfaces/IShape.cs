
namespace RayRender.Interfaces
{
    public interface IShape : IParseable
    {
        IRayHit Intersect(IRay ray);
    }
}
