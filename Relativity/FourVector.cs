using System;
using System.Collections.Generic;
using System.Text;

namespace Relativity
{
    public struct FourVector
    {
        internal FourVector(double dt, double dx)
        {
            this.Dt = dt;
            this.Dx = dx;
        }

        internal double Dt { get; }

        internal double Dx { get; }

        public double Square => Dt * Dt - Dx * Dx;

        public Coordinate In(Observer observer) => observer.Coord(this);

        public static FourVector operator +(FourVector v1, FourVector v2) => new FourVector(v1.Dt + v2.Dt, v1.Dx + v2.Dx);
        public static FourVector operator -(FourVector v1, FourVector v2) => new FourVector(v1.Dt - v2.Dt, v1.Dx - v2.Dx);
        public static FourVector operator *(FourVector v, double s) => new FourVector(v.Dt * s, v.Dx * s);
        public static FourVector operator *(double s, FourVector v) => new FourVector(v.Dt * s, v.Dx * s);
        public static FourVector operator /(FourVector v, double s) => new FourVector(v.Dt / s, v.Dx / s);
    }
}
