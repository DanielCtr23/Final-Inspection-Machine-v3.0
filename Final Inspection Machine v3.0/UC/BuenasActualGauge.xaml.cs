using System;
using System.Collections.Generic;
using System.Data;
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
        DataManager DM = new DataManager();
        public BuenasActualGauge()
        {
            InitializeComponent();
            Refresh();

        }

        public void Refresh()
        {
            TB2.Text = DateTime.Now.Hour.ToString() + ":00" + " - " + DateTime.Now.Hour.ToString() + ":59";
            DataTable dt = DM.ProduccionActual("H");
            double[] values = { int.Parse(dt.Rows[0]["Cantidad"].ToString()), int.Parse(dt.Rows[0]["Meta"].ToString()) - int.Parse(dt.Rows[0]["Cantidad"].ToString())};
            
            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.ShowLevels = false;
            Gauge.SpaceFraction = 1;
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Colors.LightGrey };
            TB.Text = values[0].ToString() + "/" + (values[0] + values[1]).ToString();
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
