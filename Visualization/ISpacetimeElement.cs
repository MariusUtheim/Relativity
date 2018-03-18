using System;
using GRaff;

namespace Relativity.Visualization
{
    public interface ISpacetimeElement
    {
        void Draw(Func<SpacetimePoint, Point> transformation);
    }
}
