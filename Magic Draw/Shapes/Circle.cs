using Johnothing.MagicDraw.Extensions;
using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Johnothing.MagicDraw.Shapes
{
    class Circle : ShapeObject
    {
        public double Radius { get; private set; }

        public override double Area => Math.PI * Math.Pow(Radius.PxToCm(), 2);

        public override double Girth => 2 * Math.PI * Radius.PxToCm();

        public Circle(double radius)
        {
            Radius = radius;
        }

        public override void Draw()
        {
            if (Instance == null || !(Instance is Ellipse))
            {
                Instance = new Ellipse();
            }

            Instance.Height = Radius * 2;
            Instance.Width = Radius * 2;
            Instance.ToolTip = new TextBlock()
            {
                Text = $"π:{Math.PI.ToString("F2")}\nRadius{Radius.PxToCm().ToString("F2")}\nGirth:{Girth.ToString("F2")}cm\nArea:{Area.ToString("F2")}cm²",
                FontSize = 12
            };
            base.Draw();
        }

        public override void Zoom(double mutilple)
        {
            if (mutilple <= 0)
            {
                throw new ArgumentOutOfRangeException($"Value must bigger than 0, current is: {mutilple}");
            }

            Radius *= mutilple;

            Draw();
        }
    }
}
