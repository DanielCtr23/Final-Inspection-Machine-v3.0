using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    /// Lógica de interacción para TiemposTurno.xaml
    /// </summary>
    public partial class TiemposTurno : UserControl
    {
        double[] values = { 0, 0 };
        DataManager DM = new DataManager();
        public TiemposTurno()
        {
            InitializeComponent();
            Refresh(false);
        }

        public void Refresh(bool TE)
        {
            if (TE)
            {
                TiempoExtra();
            }
            else
            {
                TiempoNormal();
            }

        }

        public void TiempoNormal()
        {
            Plot.Reset();
            values = new double[2];

            DataTable dt = DM.TiempoCiclo("TN");
            values[0] = double.Parse(dt.Rows[0]["Ciclo"].ToString());
            values[1] = double.Parse(dt.Rows[0]["Complemento"].ToString());
            TB2.Text = dt.Rows[0]["Horario"].ToString();
            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);

            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.SpaceFraction = 1;
            TB.Text = values[0].ToString() + "s";

            if (values[0] > 54)
            {
                Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Red, ScottPlot.Color.FromHex("#363636") };

            }
            else if (values[0] > 32 && values[0] <= 54)
            {
                Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Yellow, ScottPlot.Color.FromHex("#363636") };

            }
            else
            {
                Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };

            }
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

            Gauge.ShowLevels = false;
            Plot.Refresh();
        }

        public void TiempoExtra()
        {
            Plot.Reset();
            values = new double[2];

            DataTable dt = DM.TiempoCiclo("TE");
            values[0] = double.Parse(dt.Rows[0]["Ciclo"].ToString());
            values[1] = double.Parse(dt.Rows[0]["Complemento"].ToString());
            TB2.Text = dt.Rows[0]["Horario"].ToString();
            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);

            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.SpaceFraction = 1;
            TB.Text = values[0].ToString() + "s";

            if (values[0] > 54)
            {
                Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Red, ScottPlot.Color.FromHex("#363636") };

            }
            else if (values[0] > 32 && values[0] <= 54)
            {
                Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Yellow, ScottPlot.Color.FromHex("#363636") };

            }
            else
            {
                Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };

            }
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

            Gauge.ShowLevels = false;
            Plot.Refresh();
        }

    }
}
