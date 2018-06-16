
namespace RayRender.Interfaces
{
    public enum LightType
    {
        None,
        AmbientLight,
        PointLight
    }

    public interface ILight : IParseable
    {
        LightType Type { get; set; }

        IVector Position { get; set; }

        IColor Color { get; set; }
    }
}
