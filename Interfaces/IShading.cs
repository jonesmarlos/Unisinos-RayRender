namespace RayRender.Interfaces
{
    public interface IShading : IParseable
    {
        IPixelColor GetColor(IRayHit hit, ILight light);
    }
}
