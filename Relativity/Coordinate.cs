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

        public bool Equals(Coordinate c) => this.X == c.X && this.T == c.T;
        public override bool Equals(object obj) => obj is Coordinate ? Equals((Coordinate)obj) : false;
        public override int GetHashCode() => T.GetHashCode() ^ X.GetHashCode();
        public static bool operator ==(Coordinate c1, Coordinate c2) => c1.T == c2.T && c1.X == c2.X;
        public static bool operator !=(Coordinate c1, Coordinate c2) => c1.T != c2.T || c1.X != c2.X;
    }
}
