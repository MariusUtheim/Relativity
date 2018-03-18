using System;
using GRaff;
using static GRaff.GMath;

namespace Relativity.Visualization
{
    public class StringModel : ISpacetimeElement
    {
        public StringModel(SpacetimePoint origin, double initialEnergy, double totalEnergy, double totalMomentum)
        {
            if (initialEnergy < 0 || totalEnergy < 0 || initialEnergy > totalEnergy || GMath.Abs(totalMomentum) > totalEnergy)
                throw new ArgumentException();
            Origin = origin;
            InitialEnergy = initialEnergy;
            var v = totalMomentum / totalEnergy;
            LorentzFactor = 1 / GMath.Sqrt(1 - v * v);
            RestEnergy = LorentzFactor * (totalEnergy + v * totalMomentum);
            Momentum = totalMomentum;
        }

        public SpacetimePoint Origin { get; }
        public double InitialEnergy { get; }
        public double LeftInitialMomentum { get; }
        public double RightInitialMomentum { get; }
        public double RestEnergy { get; }
        public double Momentum { get; }
        public double Lifetime { get; set; }
        public double LorentzFactor { get; }

        private double _offset(double t)
        {
            var directEnergy = t + InitialEnergy / 2;
            var periodEnergy = GMath.Remainder(directEnergy, RestEnergy);
            if (periodEnergy > RestEnergy / 2)
                periodEnergy = RestEnergy - periodEnergy;

            return 2 * periodEnergy;
        }

        public void Draw(Func<SpacetimePoint, Point> T)
        {
            var leftVertex = Origin.ProperOffset(0, InitialEnergy / 2);
            var rightVertex = Origin.ProperOffset(0, InitialEnergy / 2);
            var leftMomentum = LeftInitialMomentum;
            var rightMomentum = RightInitialMomentum;

            var remainingLifetime = Lifetime;

            while (true)
            {
                double timeStep;
                if (leftMomentum * rightMomentum > 0)
                    timeStep = Min(Abs(leftMomentum), Abs(rightMomentum));
                else if (leftMomentum == 0 || rightMomentum == 0)
                    timeStep = Abs((leftVertex - rightVertex).In(Observer.Default).X);
                else
                    timeStep = 
            }

            var initialLeft = Origin.ProperOffset(0, -InitialEnergy / 2);
            var initialRight = Origin.ProperOffset(0, InitialEnergy / 2);

            // Draw first part (which might be broken)
            if (InitialEnergy + 2 * Lifetime < RestEnergy)
            {
                var finalLeft = Origin.ProperOffset(Lifetime, -InitialEnergy / 2 - Lifetime);
                var finalRight = Origin.ProperOffset(Lifetime, InitialEnergy / 2 + Lifetime);
                GRaff.Draw.Line(T(initialLeft), T(finalLeft), Colors.Black);
                GRaff.Draw.Line(T(initialRight), T(finalRight), Colors.Black);
                return;
            }
            else
            {
                var finalLeft = Origin.ProperOffset((RestEnergy - InitialEnergy) / 2, -RestEnergy / 2);
                var finalRight = Origin.ProperOffset((RestEnergy - InitialEnergy) / 2, RestEnergy / 2);
                GRaff.Draw.Line(T(initialLeft), T(finalLeft), Colors.Black);
                GRaff.Draw.Line(T(initialRight), T(finalRight), Colors.Black);
            }

            if (InitialEnergy + 2 * Lifetime < RestEnergy * 2)
            {
                var secondLeft = Origin.ProperOffset((RestEnergy - InitialEnergy) / 2, -RestEnergy / 2);
                var secondRight = Origin.ProperOffset((RestEnergy - InitialEnergy) / 2, RestEnergy / 2);
                var finalLeft = secondLeft.ProperOffset(Lifetime - (RestEnergy - InitialEnergy) / 2, Lifetime - (RestEnergy - InitialEnergy) / 2);
                var finalRight = secondRight.ProperOffset(Lifetime - (RestEnergy - InitialEnergy) / 2, -(Lifetime - (RestEnergy - InitialEnergy) / 2));
                GRaff.Draw.Line(T(secondLeft), T(finalLeft), Colors.Black);
                GRaff.Draw.Line(T(secondRight), T(finalRight), Colors.Black);
                return;
            }
            else
            {
                var secondLeft = Origin.ProperOffset((RestEnergy - InitialEnergy) / 2, -RestEnergy / 2);
                var secondRight = Origin.ProperOffset((RestEnergy - InitialEnergy) / 2, RestEnergy / 2);
                var final = Origin.ProperOffset(RestEnergy - InitialEnergy / 2, 0);
                GRaff.Draw.Line(T(secondLeft), T(final), Colors.Black);
                GRaff.Draw.Line(T(secondRight), T(final), Colors.Black);
            }


            // Draw all the complete parts
            var vertex = Origin.ProperOffset(RestEnergy - InitialEnergy / 2, 0);
            var remainingLifetime = Lifetime - (RestEnergy - InitialEnergy / 2);

            for (; remainingLifetime > RestEnergy; remainingLifetime -= RestEnergy)
            {
                var left = vertex.ProperOffset(RestEnergy / 2, -RestEnergy / 2);
                var right = vertex.ProperOffset(RestEnergy / 2, RestEnergy / 2);
                var nextVertex = vertex.ProperOffset(RestEnergy, 0);
                GRaff.Draw.Line(T(vertex), T(left), Colors.Black);
                GRaff.Draw.Line(T(vertex), T(right), Colors.Black);
                GRaff.Draw.Line(T(left), T(nextVertex), Colors.Black);
                GRaff.Draw.Line(T(right), T(nextVertex), Colors.Black);
                vertex = nextVertex;
            }


            // Draw final part
            if (remainingLifetime < RestEnergy / 2)
            {
                var left = vertex.ProperOffset(remainingLifetime, -remainingLifetime);
                var right = vertex.ProperOffset(remainingLifetime, remainingLifetime);
                GRaff.Draw.Line(T(vertex), T(left), Colors.Black);
                GRaff.Draw.Line(T(vertex), T(right), Colors.Black);
            }
            else
            {
                var left = vertex.ProperOffset(RestEnergy / 2, -RestEnergy / 2);
                var right = vertex.ProperOffset(RestEnergy / 2, RestEnergy / 2);
                GRaff.Draw.Line(T(vertex), T(left), Colors.Black);
                GRaff.Draw.Line(T(vertex), T(right), Colors.Black);

                remainingLifetime -= RestEnergy / 2;
                var nextLeft = left.ProperOffset(remainingLifetime, remainingLifetime);
                var nextRight = right.ProperOffset(remainingLifetime, -remainingLifetime);
                GRaff.Draw.Line(T(left), T(nextLeft), Colors.Black);
                GRaff.Draw.Line(T(right), T(nextRight), Colors.Black);
            }
        }


        public (StringModel, StringModel) Fragment(double z, double newLifetime)
        {
            if (z <= 0 || z >= 1)
                throw new ArgumentOutOfRangeException(nameof(z));

            var Et = GMath.Abs(_offset(Lifetime));
            var q = 2 * z - 1;
            var left = new StringModel(Origin.ProperOffset(Lifetime, Et/2 * (q - 1) / 2), 
                                       z * Et, 
                                       (RestEnergy - Et) / 2 + z * Et, 
                                       -(RestEnergy - Et) / 2) { Lifetime = newLifetime };
            var right = new StringModel(Origin.ProperOffset(Lifetime, Et/2 * (q + 1) / 2), 
                                        (1 - z) * Et,
                                        (RestEnergy - Et) / 2 + (1 - z) * Et, 
                                        (RestEnergy - Et) / 2) { Lifetime = newLifetime };


            return (left, right);
        }

    }
}
