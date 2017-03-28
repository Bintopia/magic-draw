using Johnothing.MagicDraw.Extensions;
using System.Windows.Controls;

namespace Johnothing.MagicDraw.Shapes
{
    class Rectangle : ShapeObject
    {
        public override double Area => Width.PxToCm() * Height.PxToCm();

        public override double Girth => 2 * (Width.PxToCm() + Height.PxToCm());

        public double Width { get; private set; }

        public double Height { get; private set; }


        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override void Draw()
        {
            if (Instance == null || !(Instance is System.Windows.Shapes.Rectangle))
            {
                Instance = new System.Windows.Shapes.Rectangle();
            }

            Instance.Width = Width;
            Instance.Height = Height;
            Instance.ToolTip = new TextBlock()
            {
                Text = $"Width:{Width.PxToCm().ToString("F2")}cm\nHeight:{Height.PxToCm().ToString("F2")}cm\nGirth:{Girth.ToString("F2")}cm\nArea:{Area.ToString("F2")}cm²",
                FontSize = 12
            };

            base.Draw();
        }

        public override void Zoom(double mutilple)
        {
            Width *= mutilple;
            Height *= mutilple;
            Draw();
        }
    }
}
