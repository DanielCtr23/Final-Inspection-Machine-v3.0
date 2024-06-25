using AdvancedHMIControls;
using MfgControl.AdvancedHMI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Lógica de interacción para Indicador.xaml
    /// </summary>
    public partial class Indicador : UserControl
    {
        public Indicador()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(string), typeof(Button), new PropertyMetadata("Ellipse", OnShapeChanged));

        // Dependency Property for Color
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.Gray, OnColorChanged));

        // Dependency Property for Text
        public static readonly DependencyProperty IndicatorTextProperty =
            DependencyProperty.Register("IndicatorText", typeof(string), typeof(Button), new PropertyMetadata("Indicator", OnTextChanged));

        public string Shape
        {
            get { return (string)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }


        private static void OnShapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicador)d;
            control.UpdateShape();
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicador)d;
            control.IndicatorEllipse.Fill = (Brush)e.NewValue;
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicador)d;
            control.IndicatorText.Text = (string)e.NewValue;
        }

        public void OK(bool s)
        {
            if (s)
            {
                IndicatorText.Text = "OK";
                this.Color = Brushes.Green;
            }
            else
            {
                IndicatorText.Text = "NOK";
                this.Color = Brushes.Red;
            }
        }

        public void Reset()
        {
            IndicatorText.Text = "";
            this.Color = Brushes.Gray;
        }

        private void UpdateShape()
        {
            if (Shape == "Ellipse")
            {
                IndicatorEllipse.Visibility = Visibility.Visible;
                // Add logic to hide other shapes if necessary
            }
            else if (Shape == "Rectangle")
            {
                IndicatorEllipse.Visibility = Visibility.Collapsed;
                // Add logic to show Rectangle if necessary
            }
        }
    }
}
