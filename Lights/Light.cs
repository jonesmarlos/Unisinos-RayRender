using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Utils;
using System;

namespace RayRender.Lights
{
    public class Light : ILight
    {
        public IVector Position { get; set; }

        public IRGBColor Color { get; set; }

        public LightType Type { get; set; }

        public Light(LightType type, IVector position, IRGBColor color)
        {
            this.Type = type;
            this.Position = position;
            this.Color = color;
        }

        public Light(LightType type) : this(type, null, null)
        {

        }
        
        public void Parse(Parameters parameters)
        {
            switch (parameters.GetName())
            {
                case "position":
                    this.Position = parameters.GetVector(1);
                    break;
                case "color":
                    this.Color = parameters.GetColor(1);
                    break;
            }
        }
    }
}
