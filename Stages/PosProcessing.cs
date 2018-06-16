using RayRender.Interfaces;
using System.Collections.Generic;

namespace RayRender.Stages
{
    public class PosProcessing : IPosProcessing
    {
        public List<IFilter> Filters { get; set; }

        public PosProcessing()
        {
            this.Filters = new List<IFilter>();
        }

        public void Execute(IWorld parameter)
        {
            this.Filters.ForEach(delegate (IFilter filter)
            {
                filter.Execute(parameter);
            });
        }
    }
}
