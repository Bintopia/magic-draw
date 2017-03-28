using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Johnothing.MagicDraw.Shapes
{
    class Line : ShapeObject
    {
        public Point A { get; set; }

        public Point B { get; set; }

        public Line(Point a, Point b)
        {
            A = a;
            B = b;
        }

        public override void Draw()
        {
            
            if (Instance == null || !(Instance is System.Windows.Shapes.Line))
            {
                Instance = new System.Windows.Shapes.Line();
            }
            var line = Instance as System.Windows.Shapes.Line;
            line.X1 = A.X;
            line.Y1 = A.Y;

            line.X2 = B.X;
            line.Y2 = B.Y;

            base.Draw();

            line.StrokeThickness = 3;
        }

        public override double Area => throw new NotImplementedException();

        public override double Girth => throw new NotImplementedException();

        public override void Zoom(double mutilple)
        {
            B = new Point(B.X * mutilple, B.Y * mutilple);
            Draw();
        }
    }
}
