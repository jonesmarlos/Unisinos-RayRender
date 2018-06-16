using RayRender.Interfaces;

namespace RayRender.Lights
{
    public class AmbientLight : Light
    {
        public AmbientLight(IVector position, IColor color) : base(position, color)
        {

        }

        public AmbientLight() : base()
        {

        }

        public override IColor GetColor(IRayHit rayHit, IRay lightRay)
        {
            IPrimitiveShape shape = rayHit.IntersectShape;
            IMaterial material = shape.Material;
            IColor ambientColor = material.Ambient;

            return this.Color.Intensify(ambientColor);
        }
    }
}
