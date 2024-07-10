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
    /// Interaction logic for BuenasActualGauge.xaml
    /// </summary>
    public partial class BuenasActualGauge : UserControl
    {
        DB db = new DB();
        public BuenasActualGauge()
        {
            InitializeComponent();

            TB2.Text = DateTime.Now.Hour.ToString() + ":00" + " - " + DateTime.Now.Hour.ToString() + ":59";
            double[] values = { db.Produccion(),0 };
            values[1] = 200 - values[0];

            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.ShowLevels = false;
            Gauge.SpaceFraction = 1;
            Gauge.Colors = new ScottPlot.Color[] {ScottPlot.Colors.Green, ScottPlot.Colors.LightGrey };
            TB.Text = values[0].ToString() + "/" + 200.ToString();
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

        }

        public void Refresh()
        {
            TB2.Text = DateTime.Now.Hour.ToString() + ":00" + " - " + DateTime.Now.Hour.ToString() + ":59";
            double[] values = { db.Produccion(), 0 };
            if (values[0]<=200)
            {
                values[1] = 200 - values[0];
            }
            else
            {
                values[1] = 0;
            }
            //Plot.Reset();
            Plot.Plot.Clear();
            var Gauge = Plot.Plot.Add.RadialGaugePlot(values); 
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.ShowLevels = false;
            Gauge.SpaceFraction = 1;
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Colors.LightGrey };
            TB.Text = values[0].ToString() + "/" + 200.ToString();
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");

            TB.Text = values[0].ToString() + "/" + 200.ToString();

            Plot.Refresh();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
