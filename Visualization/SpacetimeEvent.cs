using System;
using GRaff;

namespace Relativity.Visualization
{
    public class SpacetimeEvent : ISpacetimeElement
    {
        public SpacetimeEvent(SpacetimePoint point)
        {
            this.Point = point;
        }

        public SpacetimePoint Point { get; }

        public void Draw(Func<SpacetimePoint, Point> transformation)
        {
            GRaff.Draw.FillRectangle((transformation(Point), (0.01, 0.01)), Colors.Aqua);
        }
    }
}
