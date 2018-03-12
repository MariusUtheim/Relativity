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

        public bool Equals(SpacetimePoint c) => (this.T - c.T < 1.0e-10) && (this.X - c.X < 1.0e-10);
        public override bool Equals(object obj) => obj is SpacetimePoint ? Equals((SpacetimePoint)obj) : false;
        public override int GetHashCode() => T.GetHashCode() ^ X.GetHashCode();
        public static bool operator ==(SpacetimePoint c1, SpacetimePoint c2) => c1.Equals(c2);
        public static bool operator !=(SpacetimePoint c1, SpacetimePoint c2) => !c1.Equals(c2);

        public static FourVector operator -(SpacetimePoint p1, SpacetimePoint p2) => new FourVector(p1.T - p2.T, p1.X - p2.X);
        public static SpacetimePoint operator +(SpacetimePoint p, FourVector v) => new SpacetimePoint(p.T + v.Dt, p.X + v.Dx);
        public static SpacetimePoint operator +(FourVector v, SpacetimePoint p) => new SpacetimePoint(p.T + v.Dt, p.X + v.Dx);
        public static SpacetimePoint operator -(SpacetimePoint p, FourVector v) => new SpacetimePoint(p.T - v.Dt, p.X - v.Dx);
    }
}
