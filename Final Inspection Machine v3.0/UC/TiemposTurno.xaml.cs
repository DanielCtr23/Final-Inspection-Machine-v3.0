using SciChart.Data.Model;
using System;
using System.Collections.Generic;
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
        DB db = new DB();
        public TiemposTurno()
        {
            InitializeComponent();
            values = new double[] { 36, 36 };

            Plot.Plot.Clear();

            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.SpaceFraction = 1;
            TB.Text = values[0].ToString("F2") + "s";
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };

            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

            Gauge.ShowLevels = false;

            TiempoNormal();
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
            values = new double[2];
            if ((DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 16) || (DateTime.Now.Hour == 16 && DateTime.Now.Minute < 37))
            {
                values = new double[]{ db.TiempoCiclo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 36, 0)), 0 };
                values[1] = 72 - values[0];

                TB.Text = values[0].ToString("F2") + "s";
                TB2.Text = "7:00 - 16:36";
            }
            else if ((DateTime.Now.Hour >= 5 && DateTime.Now.Hour <= 23) || (DateTime.Now.Hour == 16 && DateTime.Now.Minute >= 37))
            {
                values = new double[]{ db.TiempoCiclo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16,37,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 1, 0, 0)), 0 };
                values[1] = 72 - values[0];

                //MessageBox.Show(values[0].ToString());


                TB.Text = values[0].ToString("F2") + "s";
                TB2.Text = "16:36 - 01:00";
            }
            else
            {
                values = new double[]{ db.TiempoCiclo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 16,37,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0)), 0 };
                values[1] = 72 - values[0];

                TB.Text = values[0].ToString("F2") + "s";
                TB2.Text = "4:36 - 01:00";

            }

            Plot.Plot.Clear();


            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.SpaceFraction = 1;
            TB.Text = values[0].ToString("F2") + "s";

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
            values = new double[2];
            if ((DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19))
            {
                values = new double[]{ db.TiempoCiclo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 59, 59)), 0 };
                values[1] = 72 - values[0];

                TB.Text = values[0].ToString("F2") + "s";
                TB2.Text = "7:00 - 19:00";
            }
            else if ((DateTime.Now.Hour >= 19 && DateTime.Now.Hour <= 23))
            {
                values = new double[]{ db.TiempoCiclo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 6, 59, 59)), 0 };
                values[1] = 72 - values[0];

                TB.Text = values[0].ToString("F2") + "s";
                TB2.Text = "19:00 - 7:00";
            }
            else
            {
                values = new double[]{ db.TiempoCiclo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 19,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 59, 59)), 0 };
                values[1] = 72 - values[0];

                TB.Text = values[0].ToString("F2") + "s";
                TB2.Text = "19:00 - 7:00";
            }


            Plot.Plot.Clear();

            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.SpaceFraction = 1;
            TB.Text = values[0].ToString("F2") + "s";

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
