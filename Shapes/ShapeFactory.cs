using RayRender.Interfaces;

namespace RayRender.Shapes
{
    public class ShapeFactory
    {
        private ShapeFactory()
        {

        }

        public static IShape Create(string shapeType)
        {
            switch (shapeType)
            {
                case "sphere":
                    return new Sphere();
                case "plane":
                    return new Plane();
            }

            return null;
        }
    }
}
