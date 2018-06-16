
namespace RayRender.Interfaces
{
    public interface IVector
    {
        float X { get; set; }

        float Y { get; set; }

        float Z { get; set; }

        float Length();

        float DistanceTo(IVector other);

        float Dot(IVector other);

        IVector Cross(IVector other);

        IVector Halfway(IVector other);

        IVector Minus(IVector other);

        IVector Negate();

        IVector Normalize();

        IVector Plus(IVector other);

        IVector Times(float scalar);
    }
}
