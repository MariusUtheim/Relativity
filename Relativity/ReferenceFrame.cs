using System;

namespace Relativity
{
    public static class ReferenceFrame
    {
        internal static double Velocity { get; private set; }

        internal static double LorentzFactor { get; private set; }

        public static IDisposable Transform(double dt, double dx, double v)
        {
            throw new NotImplementedException();
        }
    }
}
