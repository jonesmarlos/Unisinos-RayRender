using RayRender.Inputs;
using RayRender.Interfaces;
using RayRender.Outputs;
using RayRender.Renders;
using RayRender.Stages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RayRender.Core
{
    public class World : IWorld
    {
        public ICamera Camera { get; set; }

        public IImage Image { get; set; }

        public List<ILight> Lights { get; set; }

        public List<IShape> Shapes { get; set; }

        public List<IStage> Stages { get; set; }

        public World()
        {
            this.Camera = null;
            this.Image = null;
            this.Lights = new List<ILight>();
            this.Shapes = new List<IShape>();
            this.Stages = new List<IStage>();

            this.Stages.Insert(0, new Input());
            this.Stages.Insert(1, new PreProcessing());
            this.Stages.Insert(2, new Render());
            this.Stages.Insert(3, new PosProcessing());
            this.Stages.Insert(4, new Output());
        }

        public T GetStage<T>(int index)
        {
            IStage stage = this.Stages.ElementAt<IStage>(index);

            return (T)Convert.ChangeType(stage, typeof(T));
        }

        public void Execute()
        {
            this.Stages.ForEach(delegate (IStage stage)
            {
                stage.Execute(this);
            });
        }
    }
}
