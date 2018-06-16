using RayRender.Interfaces;

namespace RayRender.Maths
{
    public class Matrix : IMatrix
    {
        public float[,] Data { get; set; }

        public Matrix(float[,] data)
        {
            this.Data = data;
        }

        public IMatrix Transpose()
        {
            float[,] dataTranspose = new float[4, 4];
            dataTranspose[0, 0] = this.Data[0, 0];
            dataTranspose[1, 0] = this.Data[0, 1];
            dataTranspose[2, 0] = this.Data[0, 2];
            dataTranspose[3, 0] = this.Data[0, 3];
            dataTranspose[0, 1] = this.Data[1, 0];
            dataTranspose[1, 1] = this.Data[1, 1];
            dataTranspose[2, 1] = this.Data[1, 2];
            dataTranspose[3, 1] = this.Data[1, 3];
            dataTranspose[0, 2] = this.Data[2, 0];
            dataTranspose[1, 2] = this.Data[2, 1];
            dataTranspose[2, 2] = this.Data[2, 2];
            dataTranspose[3, 2] = this.Data[2, 3];
            dataTranspose[0, 3] = this.Data[3, 0];
            dataTranspose[1, 3] = this.Data[3, 1];
            dataTranspose[2, 3] = this.Data[3, 2];
            dataTranspose[3, 3] = this.Data[3, 3];

            return new Matrix(dataTranspose);
        }

        public IMatrix Times(IMatrix other)
        {
            float[,] dataTimes = new float[4, 4];

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    dataTimes[row, col] = this.Data[row, 0] * other.Data[0, col] + this.Data[row, 1] * other.Data[1, col] + this.Data[row, 2] * other.Data[2, col] + this.Data[row, 3] * other.Data[3, col];
                }
            }

            return new Matrix(dataTimes);
        }

        public IVector Times(IVector vector)
        {
            float x, y, z;

            x = this.Data[0, 0] * vector.X + this.Data[0, 1] * vector.Y + this.Data[0, 2] * vector.Z + this.Data[0, 3] * 1.0f;
            y = this.Data[1, 0] * vector.X + this.Data[1, 1] * vector.Y + this.Data[1, 2] * vector.Z + this.Data[1, 3] * 1.0f;
            z = this.Data[2, 0] * vector.X + this.Data[2, 1] * vector.Y + this.Data[2, 2] * vector.Z + this.Data[2, 3] * 1.0f;

            float mag = this.Data[3, 0] * vector.X + this.Data[3, 1] * vector.Y + this.Data[3, 2] * vector.Z + this.Data[3, 3] * 1.0f;

            x /= mag;
            y /= mag;
            z /= mag;

            return new Vector(x, y, z);
        }
    }
}
