using System.Collections.Generic;

namespace RayRender.Interfaces
{
    public interface IWorld
    {
        ICamera Camera { get; set; }

        IImage Image { get; set; }

        List<ILight> Lights { get; set; }

        List<IShape> Shapes { get; set; }

        List<IStage> Stages { get; set; }

        T GetStage<T>(int index);

        void Execute();
    }
}
