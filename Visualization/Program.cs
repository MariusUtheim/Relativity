using System;
using GRaff;

namespace Relativity.Visualization
{
    class MainClass
    {
        static SpacetimeDiagram diagram;
        static StringModel stringModel;

        public static void Main(string[] args)
        {
            Giraffe.Run(800, 800, gameStart);
        }

        public static void gameStart()
        {
            Instance.Create(new Background { Color = Colors.LightGray });
            GlobalEvent.ExitOnEscape = true;
            View.Rectangle(-1, 1, 2, -2).Bind();

            diagram = Instance<SpacetimeDiagram>.Create();

            stringModel = new StringModel(Observer.Default.Origin.ProperOffset(-0.8, 0), 0.631, 0, 0, 0.45);
            diagram.Elements.Add(stringModel);

            var (l, r) = stringModel.Fragment(0.6, 2.5, 2.5);
            diagram.Elements.Add(l);
            diagram.Elements.Add(r);
            //
            //(l, r) = r.Fragment(0.45, 1, 1);
            //diagram.Elements.Add(l);
            //diagram.Elements.Add(r);

            var rnd = new Random();
       //     for (int i = 0; i < 10; i++)
       //         diagram.Elements.Add(new SpacetimeEvent(Observer.Default.Point(rnd.NextDouble() - 0.5, rnd.NextDouble() - 0.5)));
            
            GlobalEvent.BeginStep += GlobalEvent_BeginStep;
        }

        static void GlobalEvent_BeginStep()
        {
            var v = GMath.Median(-0.999, (Mouse.WindowX - Window.Center.X) * 2.0 / Window.Width, 0.999);
            diagram.Observer = Observer.Default.Offset(0, 0, v);
            Window.Title = $"Observer velocity = {v}";
            //stringModel.Lifetime = (Window.Height - Mouse.WindowY) / 200.0;
            //Window.Title = stringModel.Lifetime.ToString();
        }
    }
}
