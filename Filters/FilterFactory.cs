using RayRender.Interfaces;

namespace RayRender.Filters
{
    public class FilterFactory
    {
        private FilterFactory()
        {

        }

        public static IFilter Create(string filterType)
        {
            switch (filterType)
            {
                case "blur":
                    return new Blur();
                case "grayscale":
                    return new GrayScale();
                case "inverse":
                    return new Inverse();
                case "toon":
                    return new ToonFilter();
            }

            return null;
        }
    }
}
