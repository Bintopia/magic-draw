using System;
using System.Windows;

namespace Johnothing.MagicDraw.Shapes
{
    class Line : ShapeObject
    {
        public Point A { get; set; }

        public Point B { get; set; }

        private double _a;
        private double _b;

        public Line(Point a, Point b)
        {
            A = a;
            B = b;
            _a = (A.Y - B.Y) / (A.X - b.X);
            _b = A.Y - _a * A.X;
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
            var axisX = B.X * mutilple;
            var axisY = _a * axisX + _b;
            B = new Point(axisX, axisY);
            Draw();
        }
    }
}
