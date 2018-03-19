using System;
using GRaff;
using static GRaff.GMath;

namespace Relativity.Visualization
{
    public class StringModel : ISpacetimeElement
    {
        private double _leftLeg, _rightLeg, _period; 

        public StringModel(SpacetimePoint origin, double initialEnergy, double leftMomentum, double rightMomentum, double lifetime)
        {
            this.Origin = origin;
            this.InitialEnergy = initialEnergy;
            this.LeftInitialMomentum = leftMomentum;
            this.RightInitialMomentum = rightMomentum;
            this.TotalEnergy = initialEnergy + Abs(leftMomentum) + Abs(rightMomentum);
            this.Lifetime = lifetime;
        }

        public SpacetimePoint Origin { get; }
        public double InitialEnergy { get; }
        public double LeftInitialMomentum { get; }
        public double RightInitialMomentum { get; }
        public double TotalEnergy { get; }
        public double Lifetime { get; }
        public double StringVelocity => (LeftInitialMomentum + RightInitialMomentum) / TotalEnergy;

        public void Draw(Func<SpacetimePoint, Point> T)
        {
            var leftVertex = Origin.ProperOffset(0, -InitialEnergy / 2);
            var rightVertex = Origin.ProperOffset(0, InitialEnergy / 2);

            var leftMomentum = LeftInitialMomentum;
            var rightMomentum = RightInitialMomentum;

            var remainingLifetime = Lifetime;

            while (remainingLifetime > 0)
            {
                double timeStep;
                SpacetimePoint nextLeftVertex, nextRightVertex;

                if (leftMomentum * rightMomentum < 0)
                {
                    timeStep = Min(Abs(leftMomentum), Abs(rightMomentum));
                    var s = Sign(leftMomentum);
                    nextLeftVertex = leftVertex.ProperOffset(timeStep, timeStep * s);
                    nextRightVertex = rightVertex.ProperOffset(timeStep, -timeStep * s);
                }
                else if (leftMomentum == 0 && rightMomentum == 0)
                {
                    timeStep = TotalEnergy / 2;
                    // ensure that next left is to the right of the current left, in case
                    // we are currently at maximal energy
                    nextLeftVertex = leftVertex.ProperOffset(timeStep, timeStep);
                    nextRightVertex = rightVertex.ProperOffset(timeStep, -timeStep);
                }
                else if (leftMomentum * rightMomentum > 0)
                {
                    var s = Sign(leftMomentum);
                    timeStep = Min(Abs(leftMomentum), Abs(rightMomentum));
                    nextLeftVertex = leftVertex.ProperOffset(timeStep, s * timeStep);
                    nextRightVertex = rightVertex.ProperOffset(timeStep, s * timeStep);
                }
                else // if one momentum is zero, but not both
                {
                    if (leftMomentum == 0)
                    {
                        if (rightMomentum > 0)
                        {
                            timeStep = rightMomentum;
                            nextLeftVertex = leftVertex.ProperOffset(timeStep, rightMomentum);
                            nextRightVertex = rightVertex.ProperOffset(timeStep, rightMomentum);
                        }
                        else
                        {
                            timeStep = Abs((rightVertex.X - leftVertex.X) / 2);
                            nextLeftVertex = leftVertex.ProperOffset(timeStep, timeStep);
                            nextRightVertex = rightVertex.ProperOffset(timeStep, -timeStep);
                        }
                    }
                    else // if (rightMomentum == 0)
                    {
                        if (leftMomentum < 0)
                        {
                            timeStep = Abs(leftMomentum);
                            nextLeftVertex = leftVertex.ProperOffset(timeStep, leftMomentum);
                            nextRightVertex = rightVertex.ProperOffset(timeStep, -timeStep);
                        }
                        else
                        {
                            timeStep = (rightVertex.X - leftVertex.X) / 2;
                            nextLeftVertex = leftVertex.ProperOffset(timeStep, timeStep);
                            nextRightVertex = rightVertex.ProperOffset(timeStep, -timeStep);
                        }
                    }
                }

                if (timeStep <= remainingLifetime)
                {
                    GRaff.Draw.FillTriangle(T(leftVertex), T(nextLeftVertex), T(nextRightVertex), Colors.Orange);
                    GRaff.Draw.FillTriangle(T(leftVertex), T(nextRightVertex), T(rightVertex), Colors.Orange);
                    GRaff.Draw.Line(T(leftVertex), T(nextLeftVertex), Colors.Black);
                    GRaff.Draw.Line(T(rightVertex), T(nextRightVertex), Colors.Black);

                    leftMomentum += timeStep;
                    rightMomentum -= timeStep;
                                        
                    if (nextLeftVertex.X < nextRightVertex.X)
                        (leftVertex, rightVertex) = (nextLeftVertex, nextRightVertex);
                    else if (nextLeftVertex.X == nextRightVertex.X)
                    {
                        (leftVertex, rightVertex) = (nextRightVertex, nextLeftVertex);
                        (leftMomentum, rightMomentum) = (rightMomentum, leftMomentum);
                    }
                    else
                        throw new Exception();
                    
                    remainingLifetime -= timeStep;
                }
                else
                {
                    nextLeftVertex = leftVertex.ProperOffset(remainingLifetime, (leftMomentum < 0 ? -1 : +1) * remainingLifetime);
                    nextRightVertex = rightVertex.ProperOffset(remainingLifetime, (rightMomentum > 0 ? +1 : -1) * remainingLifetime);
                    GRaff.Draw.FillTriangle(T(leftVertex), T(nextLeftVertex), T(nextRightVertex), Colors.Orange);
                    GRaff.Draw.FillTriangle(T(leftVertex), T(nextRightVertex), T(rightVertex), Colors.Orange);
                    GRaff.Draw.Line(T(leftVertex), T(nextLeftVertex), Colors.Black);
                    GRaff.Draw.Line(T(rightVertex), T(nextRightVertex), Colors.Black);
                    break;
                }

            }

            //var t = Mouse.WindowY / (double)Window.Height;
            //GRaff.Draw.FillRectangle(T(Origin.ProperOffset(t, _leftZ(t))), (0.01, 0.01), Colors.Red);
            //GRaff.Draw.FillRectangle(T(Origin.ProperOffset(t, _rightZ(t))), (0.01, 0.01), Colors.Blue);
            //
            //for (var i = 0; i < 3; i++)
            //    GRaff.Draw.FillRectangle(T(Origin.ProperOffset(i * (h + k) - tOffset, (i * (h + k) - tOffset) * StringVelocity)), (0.01, 0.01), Colors.Black);

        }


        private double q => Sqrt((1 + StringVelocity) / (1 - StringVelocity));
        private double h => 0.5 * TotalEnergy * q;
        private double k => 0.5 * TotalEnergy / q;
        private double tOffset
        {
            get
            {
                if (LeftInitialMomentum == 0 && RightInitialMomentum == 0)
                    return InitialEnergy / 2;
                else if (LeftInitialMomentum < 0 && RightInitialMomentum > 0)
                    return InitialEnergy / 2;
                else if (LeftInitialMomentum >= 0 && RightInitialMomentum <= 0)
                    return h + k - InitialEnergy;
                else if (LeftInitialMomentum < 0 && RightInitialMomentum < 0)
                    return -RightInitialMomentum + h;
                else // if (LeftInitialMomentum > 0 && RightInitialMomentum > 0)
                    return LeftInitialMomentum + k;
            }
        }

        private double _leftZ(double t)
        {
            t += tOffset;

            var remainder = (t % (h + k));
            var halfPeriods = (t - remainder) / (h + k);

            if (remainder < k)
                return -(k - h) * halfPeriods - remainder;
            else
                return -(k - h) * halfPeriods - k + (remainder - k);
        }

        private double _rightZ(double t)
        {
            t += tOffset;

            var remainder = (t % (h + k));
            var halfPeriods = (t - remainder) / (h + k);

            if (remainder < h)
                return (h - k) * halfPeriods + remainder;
            else
                return (h - k) * halfPeriods + h - (remainder - h);
        }

        private double _leftP(double t)
        {
            t -= tOffset;

            var remainder = (t % (2 * h + 2 * k));

            if (remainder < h + k)
                return -k + remainder;
            else
                return h - (remainder - h - k);
        }

        private double _rightP(double t)
        {
            t -= tOffset;

            var remainder = (t % (2 * h + 2 * k));

            if (remainder < h + k)
                return h - remainder;
            else
                return -k + (remainder - h - k);
        }



        public (StringModel, StringModel) Fragment(double z, double leftLifetime, double rightLifetime)
        {
            if (z <= 0 || z >= 1)
                throw new ArgumentOutOfRangeException(nameof(z));
            
            var leftOffset = _leftZ(Lifetime);
            var rightOffset = _rightZ(Lifetime);
            var Et = rightOffset - leftOffset;
            var center = (leftOffset + rightOffset) / 2.0;

            var q = 2 * z - 1;
            var left = new StringModel(Origin.ProperOffset(Lifetime, center + Et / 2 * (q - 1) / 2),
                                        z * Et,
                                        _leftP(Lifetime), 0, leftLifetime);
            var right = new StringModel(Origin.ProperOffset(Lifetime, center + Et / 2 * (q + 1) / 2),
                                         (1 - z) * Et,
                                         0, _rightP(Lifetime), rightLifetime);

            return (left, right);
        }
       
    }

}
