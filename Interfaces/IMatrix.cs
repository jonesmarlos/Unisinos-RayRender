
namespace RayRender.Interfaces
{
    public interface IMatrix
    {
        float[,] Data { get; set; }

        IMatrix Transpose();

        IMatrix Times(IMatrix other);

        IVector Times(IVector vector);
    }
}
