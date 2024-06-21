using SciChart.Data.Model;
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
    /// Interaction logic for BuenasTurno.xaml
    /// </summary>
    public partial class BuenasTurno : UserControl
    {
        DB db = new DB();
        bool TE = false;
        
        public BuenasTurno()
        {
            InitializeComponent(); 
            double[] values = {0 , 0 };
            values[1] = 2100 - values[0];


            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.ShowLevels = false;
            Gauge.SpaceFraction = 1;
            TB.Text = values[0].ToString() + "/" + 2100.ToString();
            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };

            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;
            TiempoNormal();
        }

        public void Refresh()
        {
            if (TE)
            {
                TiempoExtra();
                TE = false;
            }
            else
            {
                TiempoNormal();
                TE = true;
            }
        }

        private void TiempoNormal()
        {
            double[] values = new double[2];
            Plot.Reset();
            if ((DateTime.Now.Hour>=7 && DateTime.Now.Hour <16) || (DateTime.Now.Hour == 16 && DateTime.Now.Minute < 37) )
            {
                values = new double[]{ db.Produccion(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7,0,0), 
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 36, 0)), 0 };
                values[1] = 2100 - values[0];

                TB.Text = values[0].ToString() + "/" + 2100.ToString();
                TB2.Text = "7:00 - 16:36";
            }
            else if((DateTime.Now.Hour >= 5 && DateTime.Now.Hour <= 23) || (DateTime.Now.Hour == 16 && DateTime.Now.Minute >= 37))
            {
                values = new double[]{ db.Produccion(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 4,37,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 1, 0, 0)), 0 };
                values[1] = 2100 - values[0];

                TB.Text = values[0].ToString() + "/" + 2100.ToString();
                TB2.Text = "4:36 - 01:00";
            }
            else
            {
                values = new double[]{ db.Produccion(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 4,37,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0)), 0 };
                values[1] = 2100 - values[0];

                TB.Text = values[0].ToString() + "/" + 2100.ToString();
                TB2.Text = "4:36 - 01:00";
            }

            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.ShowLevels = false;
            Gauge.SpaceFraction = 1;

            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };

            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

        }

        private void TiempoExtra()
        {
            double[] values = new double[2];
            Plot.Reset();
            if ((DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19))
            {
                values = new double[]{ db.Produccion(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 59, 59)), 0 };
                values[1] = 2100 - values[0];

                TB.Text = values[0].ToString() + "/" + 2100.ToString();
                TB2.Text = "7:00 - 19:00";
            }
            else if ((DateTime.Now.Hour >= 19 && DateTime.Now.Hour < 24))
            {
                values = new double[]{ db.Produccion(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 6, 59, 59)), 0 };
                values[1] = 2100 - values[0];

                TB.Text = values[0].ToString() + "/" + 2100.ToString();
                TB2.Text = "19:00 - 7:00";
            }
            else
            {
                values = new double[]{ db.Produccion(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 19,0,0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 59, 59)), 0 };
                values[1] = 2100 - values[0];

                TB.Text = values[0].ToString() + "/" + 2100.ToString();
                TB2.Text = "19:00 - 7:00";
            }

            var Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge = Plot.Plot.Add.RadialGaugePlot(values);
            Gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            Gauge.Clockwise = true;
            Gauge.OrderInsideOut = true;
            Gauge.MaximumAngle = 270;
            Gauge.StartingAngle = 135;
            Gauge.ShowLevels = false;
            Gauge.SpaceFraction = 1;

            Gauge.Colors = new ScottPlot.Color[] { ScottPlot.Colors.Green, ScottPlot.Color.FromHex("#363636") };

            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Refresh();
        }
    }
}
