using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Maths;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayRender.Utils
{
    public class Parameters : List<string>
    {
        public string GetString(int index)
        {
            return this.ElementAtOrDefault<string>(index);
        }

        public int GetInt(int index)
        {
            return int.Parse(this.GetString(index));
        }

        public bool GetBool(int index)
        {
            return bool.Parse(this.GetString(index));
        }

        public float GetFloat(int index)
        {
            string value = this.GetString(index);

            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(",", "");
                value = value.Replace(".", ",");

                return float.Parse(value);
            }
            return float.NaN;
        }

        public float[] GetFloats(int startIndex)
        {
            return this.GetFloats(startIndex, this.Count);
        }

        public float[] GetFloats(int startIndex, int size)
        {
            float[] result = new float[size];

            for (int index = 0; index < size; index++)
            {
                result[index] = this.GetFloat(startIndex + index);
            }

            return result;
        }

        public IVector GetVector(int startIndex)
        {
            float[] xyz = this.GetFloats(startIndex, 3);

            return new Vector(xyz[0], xyz[1], xyz[2]);
        }

        public IRGBColor GetColor(int startIndex)
        {
            float[] rgb = this.GetFloats(startIndex, 3);

            return new RGBColor(rgb[0], rgb[1], rgb[2]);
        }

        public string GetName()
        {
            return this.GetString(0);
        }

        public override string ToString()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(this.GetString(0));

            for (int index = 1; index < this.Count; index++)
            {
                stringBuilder.Append(';');
                stringBuilder.Append(this.GetString(index));
            }

            return stringBuilder.ToString();
        }
    }
}
