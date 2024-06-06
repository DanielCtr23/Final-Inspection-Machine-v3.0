using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Lógica de interacción para Chart270UC.xaml
    /// </summary>
    public partial class Chart270UC : UserControl
    {
        public Chart270UC()
        {
            InitializeComponent();
            InitializeArc();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Chart270UC),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty Text1Property =
            DependencyProperty.Register("Text1", typeof(string), typeof(Chart270UC),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnText1Changed)));

        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(Chart270UC),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnText2Changed)));

        public static readonly DependencyProperty BackgroundArcColorProperty =
           DependencyProperty.Register("BackgroundArcColor", typeof(Color), typeof(Chart270UC),
               new PropertyMetadata(Colors.LightGray, new PropertyChangedCallback(OnBackgroundArcColorChanged)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        public string Text1
        {
            get { return (string)GetValue(Text1Property); }
            set { SetValue(Text1Property, value); }
        }

        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }

        public Color BackgroundArcColor
        {
            get { return (Color)GetValue(BackgroundArcColorProperty); }
            set { SetValue(BackgroundArcColorProperty, value); }
        }

        private double minValue = 0;
        private double maxValue = 100;


        private void InitializeArc()
        {
            DrawArc(backgroundArc, 135, 405, 100, BackgroundArcColor);

            // Actualización inicial
            UpdateArc(Value);
        }


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart270UC control = d as Chart270UC;
            control.UpdateArc((double)e.NewValue);
        }

        private static void OnText1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart270UC control = d as Chart270UC;
            control.text1.Text = (string)e.NewValue;
        }

        private static void OnText2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart270UC control = d as Chart270UC;
            control.text2.Text = (string)e.NewValue;
        }
        private static void OnBackgroundArcColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart270UC control = d as Chart270UC;
            control.backgroundArc.Stroke = new SolidColorBrush((Color)e.NewValue);
        }


        private void UpdateArc(double value)
        {
            // Calculate the angle for the current value
            double angle = 135 + (270 * (value - minValue) / (maxValue - minValue));

            // Update the foreground arc
            DrawArc(foregroundArc, 135, angle, 100, Colors.Green);
        }

        private void DrawArc(Path path, double startAngle, double endAngle, double radius, Color color)
        {
            double centerX = canvas.Width / 2;
            double centerY = canvas.Height / 2;

            double startAngleRad = startAngle * Math.PI / 180;
            double endAngleRad = endAngle * Math.PI / 180;

            Point startPoint = new Point(
                centerX + radius * Math.Cos(startAngleRad),
                centerY + radius * Math.Sin(startAngleRad));

            Point endPoint = new Point(
                centerX + radius * Math.Cos(endAngleRad),
                centerY + radius * Math.Sin(endAngleRad));

            bool isLargeArc = endAngle - startAngle > 180;

            PathFigure figure = new PathFigure
            {
                StartPoint = startPoint,
                Segments = {
            new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = isLargeArc
            }
        }
            };

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            path.Data = geometry;
            path.Stroke = new SolidColorBrush(color);
        }
    }
}
