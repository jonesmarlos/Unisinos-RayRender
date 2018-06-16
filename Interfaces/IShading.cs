namespace RayRender.Interfaces
{
    public interface IShading : IParseable
    {
        IColor GetColor(IRayHit hit, ILight light);
    }
}
