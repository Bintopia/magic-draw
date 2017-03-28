using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Johnothing.MagicDraw.Shapes
{
    abstract class ShapeObject : IShape
    {
        public Brush BorderColorBrush { get; set; }

        public Brush Background { get; set; }

        public double Top { get; protected set; }

        public double Left { get; protected set; }

        public Shape Instance { get; protected set; }

        public abstract double Area { get; }

        public abstract double Girth { get; }

        public virtual void Draw()
        {
            if (Instance != null)
            {
                Instance.Stroke = BorderColorBrush ?? Brushes.SeaShell;
                Instance.Fill = Background ?? Brushes.SeaShell;
                Instance.Focusable = true;
                Instance.AllowDrop = true;
            }
        }

        public void Move(double xOffset = 0, double yOffset = 0)
        {
            Top += yOffset;
            Left += xOffset;
            Draw();
        }

        public void Move(Point destination)
        {
            Top = destination.Y;
            Left = destination.X;
            Draw();
        }

        public abstract void Zoom(double mutilple);

        public void ZoomIn(double mutilple)
        {
            if (mutilple < 1)
            {
                throw new ArgumentOutOfRangeException($"Must bigger than 1, current is: {mutilple}");
            }

            Zoom(mutilple);
        }

        public void ZoomOut(double mutilple)
        {
            if (mutilple > 1 || mutilple <= 0)
            {
                throw new ArgumentOutOfRangeException($"Must between 0 and 1, current is: {mutilple}");
            }
            Zoom(mutilple);
        }
    }
}