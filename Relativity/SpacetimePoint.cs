using System;

namespace Relativity
{
    public struct SpacetimePoint
    {
        private double _t, _x;

        internal SpacetimePoint(double t, double x)
        {
            this._t = t;
            this._x = x;
        }

        public SpacetimePoint Create(double t, double x)
        {
            return new SpacetimePoint(ReferenceFrame.LorentzFactor * (t + ReferenceFrame.Velocity * x),
                                      ReferenceFrame.LorentzFactor * (x + ReferenceFrame.Velocity * t));
        }

        public double T => _t;//ReferenceFrame.LorentzFactor * (_t - ReferenceFrame.Velocity * _x);
        public double X => _x;//ReferenceFrame.LorentzFactor * (_x - ReferenceFrame.Velocity * _t);

        public Coordinate In(Observer observer) => observer.Coord(this);

        public SpacetimePoint ProperOffset(double dt, double dx) => new SpacetimePoint(T + dt, X + dx);

        public SpacetimePoint Boosted(double v)
        {
            if (v <= -1 || v >= 1)
                throw new ArgumentOutOfRangeException(nameof(v), "Trying to boost to faster than speed of light");
            var g = 1 / Math.Sqrt(1 - v * v);
            return new SpacetimePoint(g * (T - v * X), g * (X - v * T));
        }

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
