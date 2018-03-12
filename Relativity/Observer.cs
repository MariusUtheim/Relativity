using System;
using System.Collections.Generic;
using System.Text;

namespace Relativity
{
    public class Observer
    {
        private double _v;

        private static double _sqr(double x) => x * x;

        private Observer(double t, double x, double v)
        {
            this._v = v;
            Origin = new SpacetimePoint(t, x);
        }

        public static Observer Default { get; } = new Observer(0, 0, 0);

        public SpacetimePoint Origin { get; }

        public double Velocity(Observer rest) => (_v - rest._v) / (1 - _v * rest._v);

        public double LorentzFactor(Observer rest) => 1 / Math.Sqrt(1 - _sqr(Velocity(rest)));

        public Observer Offset(double originT, double originX, double v)
        {
            if (v < -1 || v > 1)
                throw new ArgumentOutOfRangeException(nameof(v), "Observer moving faster than speed of light");
            return new Observer(originT - this.Origin.T, originX - this.Origin.X, (_v + v) / (1 + _v * v));
        }

        public Coordinate Coord(SpacetimePoint p) => new Coordinate(p.T - Origin.T, p.X - Origin.X);
    }
}
