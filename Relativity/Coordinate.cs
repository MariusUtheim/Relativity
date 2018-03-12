using System;
using System.Collections.Generic;
using System.Text;

namespace Relativity
{
    public struct Coordinate
    {
        public Coordinate(double t, double x)
        {
            T = t;
            X = x;
        }

        public double T { get; }
        public double X { get; }

        public bool Equals(Coordinate c) => (this.T - c.T < 1.0e-10) && (this.X - c.X < 1.0e-10);
        public override bool Equals(object obj) => obj is Coordinate ? Equals((Coordinate)obj) : false;
        public override int GetHashCode() => T.GetHashCode() ^ X.GetHashCode();
        public static bool operator ==(Coordinate c1, Coordinate c2) => c1.Equals(c2);
        public static bool operator !=(Coordinate c1, Coordinate c2) => !c1.Equals(c2);

        public override string ToString() => $"({T}; {X})";
    }
}
