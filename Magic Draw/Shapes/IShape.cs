using System.Windows;

namespace Johnothing.MagicDraw.Shapes
{
    public interface IShape
    {
        void Draw();

        void Move(Point destination);

        void Move(double xOffset = 0, double yOffset = 0);

        void ZoomIn(double mutilple);

        void ZoomOut(double mutilple);
    }
}
