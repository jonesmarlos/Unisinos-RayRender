
namespace RayRender.Interfaces
{
    public interface IColor
    {
        float Red { get; set; }

        float Green { get; set; }

        float Blue { get; set; }

        IColor Blend(IColor other);

        IColor Intensify(IColor other);

        IColor Intensify(float scalar);
    }
}
