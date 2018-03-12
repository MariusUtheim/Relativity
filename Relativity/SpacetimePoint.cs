using System;

namespace Relativity
{
    public struct SpacetimePoint
    {
        internal SpacetimePoint(double t, double x)
        {
            this.T = t;
            this.X = x;
        }

        internal double T { get; }
        internal double X { get; }

        public Coordinate In(Observer observer) => observer.Coord(this);
    }
}
