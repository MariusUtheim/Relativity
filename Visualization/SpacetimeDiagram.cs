using System;
using System.Collections.Generic;
using GRaff;

namespace Relativity.Visualization
{
    public class SpacetimeDiagram : GameElement
    {
        public SpacetimeDiagram()
        {
        }

        public Func<SpacetimePoint, Point> Transform => p => { var c = p.In(_observer); return (c.X, c.T); };

        public List<ISpacetimeElement> Elements = new List<ISpacetimeElement>();

        private Observer _observer = Observer.Default;
        public Observer Observer
        {
            get => _observer;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _observer = value;
            }
        }


        public override void OnDraw()
        {
            foreach (var element in Elements)
                element.Draw(Transform);
        }
    }
}
