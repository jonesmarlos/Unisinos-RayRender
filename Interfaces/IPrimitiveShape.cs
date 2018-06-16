
namespace RayRender.Interfaces
{
    public interface IPrimitiveShape : IShape
    {
        IMaterial Material { get; set; }

        IVector Position { get; set; }
    }
}
