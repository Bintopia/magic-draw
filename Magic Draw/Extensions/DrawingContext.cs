using Johnothing.MagicDraw.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Johnothing.MagicDraw.Extensions
{
    class DrawingContext : NotificationObject
    {
        public DrawingContext()
        {
            Clear();
            ColorBrush = new SolidColorBrush(Colors.Tomato);
        }

        private bool _isColorBrushToggled;

        public bool IsColorBrushToggled
        {
            get { return _isColorBrushToggled; }
            set
            {
                _isColorBrushToggled = value;
                RaisePropertyChanged(() => IsColorBrushToggled);
            }
        }


        private SolidColorBrush _colorBrush;

        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            set
            {
                _colorBrush = value;
                RaisePropertyChanged(() => ColorBrush);
            }
        }


        private bool _canDrawing;
        public bool CanDrawing
        {
            get { return _canDrawing; }
            private set
            {
                _canDrawing = value;
                RaisePropertyChanged(() => CanDrawing);
            }
        }

        private Point _coordinates;
        public Point Coordinates
        {
            get { return _coordinates; }
            set
            {
                _coordinates = value;
                RaisePropertyChanged(() => Coordinates);
            }
        }

        private Point _initiMousePoint;
        public Point InitiMousePoint
        {
            get { return _initiMousePoint; }
            set
            {
                _initiMousePoint = value;
                RaisePropertyChanged(() => InitiMousePoint);
            }
        }

        private ShapeType _shapeType;

        public ShapeType ShapeType
        {
            get { return _shapeType; }
            set
            {
                _shapeType = value;
                CanDrawing = _shapeType != ShapeType.None;
                RaisePropertyChanged(() => ShapeType);
            }
        }

        public void Clear()
        {
            ShapeType = ShapeType.None;
            CanDrawing = false;
            InitiMousePoint = new Point(-1, -1);
            Coordinates = new Point(-1, -1);
            IsColorBrushToggled = false;
        }
    }
}
