using Johnothing.MagicDraw.Extensions;
using Johnothing.MagicDraw.Shapes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Johnothing.MagicDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Selected shape's top on canvas before move.
        private double _canvasTop;

        // Selected shape's left on canvas before move.
        private double _canvasLeft;

        private Point _mouseEndPoint;

        private List<ShapeObject> _shapes = new List<ShapeObject>();

        private Extensions.DrawingContext _drawingContext;

        public MainWindow()
        {
            InitializeComponent();

            _drawingContext = new Extensions.DrawingContext();
            DataContext = _drawingContext;
        }

        private void Stage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_drawingContext.CanDrawing)
            {
                if (e.ClickCount == 1)
                {
                    _drawingContext.InitiMousePoint = e.GetPosition(Stage);
                }
            }
        }

        private void Stage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && _drawingContext.CanDrawing)
            {
                _mouseEndPoint = e.GetPosition(Stage);
                Draw(_drawingContext.ShapeType);
            }

            ClearDrawingContext();
        }

        private void Stage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drawingContext.CanDrawing)
            {
                var current = e.MouseDevice.GetPosition(Stage);
                _drawingContext.Coordinates = current;
            }
        }

        private double GetDistance()
        {
            return Math.Sqrt(Math.Pow(_drawingContext.InitiMousePoint.X - _mouseEndPoint.X, 2) + Math.Pow(_drawingContext.InitiMousePoint.Y - _mouseEndPoint.Y, 2));
        }

        private Point GetMid()
        {
            var x = (_mouseEndPoint.X - _drawingContext.InitiMousePoint.X) / 2 + _mouseEndPoint.X;
            var y = (_mouseEndPoint.Y - _drawingContext.InitiMousePoint.Y) / 2 + _mouseEndPoint.Y;
            return new Point(x, y);
        }

        private void Draw(ShapeType type)
        {
            ShapeObject shape = null;
            switch (type)
            {
                case ShapeType.Circle:
                    shape = new Circle(GetDistance() / 2);
                    _canvasLeft = _drawingContext.InitiMousePoint.X;
                    _canvasTop = _drawingContext.InitiMousePoint.Y;
                    break;
                case ShapeType.Rectangle:
                    shape = new Rectangle(_mouseEndPoint.X - _drawingContext.InitiMousePoint.X, _mouseEndPoint.Y - _drawingContext.InitiMousePoint.Y);
                    _canvasLeft = _drawingContext.InitiMousePoint.X;
                    _canvasTop = _drawingContext.InitiMousePoint.Y;
                    break;
                case ShapeType.Line:
                    shape = new Line(_drawingContext.InitiMousePoint, _mouseEndPoint);
                    _canvasLeft = _drawingContext.InitiMousePoint.X;
                    _canvasTop = _drawingContext.InitiMousePoint.Y;
                    break;
                default:
                    shape = null;
                    break;
            }

            if (shape != null)
            {
                shape.BorderColorBrush = _drawingContext.ColorBrush;
                shape.Background = _drawingContext.ColorBrush;
                shape.Draw();
                if (!(shape is Line))
                {
                    shape.Instance.SetValue(Canvas.LeftProperty, _canvasLeft);
                    shape.Instance.SetValue(Canvas.TopProperty, _canvasTop);
                }

                shape.Instance.MouseUp += ShapeInstance_MouseUp;
                shape.Instance.LostFocus += ShapeInstance_LostFocus;

                shape.Instance.MouseLeftButtonDown += ShapeInstance_MouseLeftButtonDown;
                shape.Instance.MouseMove += ShapeInstance_MouseMove;

                _shapes.Add(shape);
                Stage.Children.Add(shape.Instance);

                SetShapeLogicalFocus(shape);
            }
        }

        private void ShapeInstance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && sender is System.Windows.Shapes.Shape shapeInstance)
            {
                _drawingContext.InitiMousePoint = e.GetPosition(Stage);
                var shape = _shapes.Find(s => s.Instance == shapeInstance);
                if (shape is Line)
                {
                    _canvasLeft = (shape as Line).A.X;
                    _canvasTop = (shape as Line).A.Y;
                }
                else
                {
                    _canvasLeft = (double)shapeInstance.GetValue(Canvas.LeftProperty);
                    _canvasTop = (double)shapeInstance.GetValue(Canvas.TopProperty);
                }
            }
        }

        private void ShapeInstance_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is System.Windows.Shapes.Shape shapeInstance)
            {
                var current = e.GetPosition(Stage);
                var xOffset = current.X - _drawingContext.InitiMousePoint.X;
                var yOffset = current.Y - _drawingContext.InitiMousePoint.Y;
                shapeInstance.SetValue(Canvas.LeftProperty, xOffset + _canvasLeft);
                shapeInstance.SetValue(Canvas.TopProperty, yOffset + _canvasTop);
            }
        }

        private void ShapeInstance_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled) return;

            if (sender is System.Windows.Shapes.Shape shapeInstance)
            {
                var shape = _shapes.Find(s => s.Instance == shapeInstance);

                if (_drawingContext.IsColorBrushToggled)
                {
                    shape.Background = _drawingContext.ColorBrush;
                    shape.Draw();
                }

                SetShapeLogicalFocus(shape);
            }
        }

        private void ShapeInstance_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Shapes.Shape shapeInstance)
            {
                var shape = _shapes.Find(s => s.Instance == shapeInstance);
                RemoveShapeLogicalFocus(shape);
            }
        }

        private void SetShapeLogicalFocus(ShapeObject shape)
        {
            shape.BorderColorBrush = Brushes.LightSeaGreen;
            shape.Instance.Stroke = shape.BorderColorBrush;
            if (!(shape is Line))
            {
                var strokeDashArry = new DoubleCollection { 2, 2 };
                shape.Instance.StrokeDashArray = strokeDashArry;
            }

            shape.Instance.StrokeThickness = 3;
            FocusManager.SetFocusedElement(Stage, shape.Instance);
        }

        private void RemoveShapeLogicalFocus(ShapeObject shape)
        {
            shape.BorderColorBrush = _drawingContext.ColorBrush;
            if (!(shape is Line))
            {
                shape.Instance.StrokeThickness = 0.1;
            }
            shape.Instance.StrokeDashArray = null;
            shape.Instance.Stroke = shape.BorderColorBrush;
        }

        private void RemoveShapeLogicalFocus()
        {
            var focusedElement = GetLogicalFocusedElementOnStage();
            if (focusedElement != null)
            {
                RemoveShapeLogicalFocus(focusedElement);
            }
        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            if (btnCircle.IsChecked == true)
            {
                RemoveShapeLogicalFocus();
                _drawingContext.Clear();

                btnRectangle.IsChecked = false;
                btnLine.IsChecked = false;

                _drawingContext.ShapeType = ShapeType.Circle;
            }
            else
            {
                ClearDrawingContext();
            }
        }

        private void Line_Click(object sender, RoutedEventArgs e)
        {
            if (btnLine.IsChecked == true)
            {
                RemoveShapeLogicalFocus();
                _drawingContext.Clear();

                btnRectangle.IsChecked = false;
                btnCircle.IsChecked = false;

                _drawingContext.ShapeType = ShapeType.Line;
            }
            else
            {
                ClearDrawingContext();
            }
        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            if (btnRectangle.IsChecked == true)
            {
                RemoveShapeLogicalFocus();
                _drawingContext.Clear();

                btnCircle.IsChecked = false;
                btnLine.IsChecked = false;

                _drawingContext.ShapeType = ShapeType.Rectangle;
            }
            else
            {
                ClearDrawingContext();
            }
        }

        private ShapeObject GetLogicalFocusedElementOnStage()
        {
            var focusedElement = FocusManager.GetFocusedElement(Stage);
            if (focusedElement is System.Windows.Shapes.Shape shapeInstance)
            {
                return _shapes.Find(s => s.Instance == shapeInstance);
            }
            return null;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            var target = GetLogicalFocusedElementOnStage();
            if (target == null)
            {
                StatusMessage.Content = "Please select a target for zoom out.";
            }
            else
            {
                target.ZoomOut(0.9);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            var target = GetLogicalFocusedElementOnStage();
            if (target == null)
            {
                StatusMessage.Content = "Please select a target for zoom in.";
            }
            else
            {
                target.ZoomIn(1.1);
            }
        }

        private void ColorBrush_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;
            if (Keyboard.Modifiers == ModifierKeys.Control && _drawingContext.IsColorBrushToggled == true)
            {
                var colorPicker = new System.Windows.Forms.ColorDialog();
                if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _drawingContext.ColorBrush = new SolidColorBrush(new Color()
                    {
                        R = colorPicker.Color.R,
                        G = colorPicker.Color.G,
                        B = colorPicker.Color.B,
                        A = colorPicker.Color.A
                    });
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearDrawingContext();
            RemoveShapeLogicalFocus();
        }

        private void ClearDrawingContext()
        {
            _drawingContext.Clear();
            btnCircle.IsChecked = false;
            btnRectangle.IsChecked = false;
            btnLine.IsChecked = false;
        }
    }
}
