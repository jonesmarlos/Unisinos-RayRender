
namespace RayRender.Interfaces
{
    public interface IRayHit
    {
        IRay Ray { get; set; }

        IShape MasterShape { get; set; }

        IPrimitiveShape IntersectShape { get; set; }

        IVector Normal { get; set; }

        IVector Intersection { get; set; }

        float Time { get; set; }

        IRay GetReflectionRay();

        IRay GetRefractionRay();
    }
}
