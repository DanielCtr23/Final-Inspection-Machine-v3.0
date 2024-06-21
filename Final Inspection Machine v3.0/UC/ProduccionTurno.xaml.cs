using ScottPlot;
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
    /// Interaction logic for ProduccionTurno.xaml
    /// </summary>
    public partial class ProduccionTurno : UserControl
    {
        bool TE;
        DB db = new DB();
        DataTable produccion;
        public ProduccionTurno()
        {
            InitializeComponent();
            TE = true;
            TiempoExtra();
        }


        private void TiempoExtra()
        {
            Bar[] bars = new Bar[12];
            Tick[] ticks = new Tick[12];

            Random rand = new Random();

            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 19)
            {
                produccion = db.Produccion(2);
            }
            else
            {
                produccion = db.Produccion(4);
            }


            for (int i = 0; i < 12; i++)
            {
                bars[i] = new Bar() { Position = i, Value = int.Parse(produccion.Rows[i]["Buenas"].ToString()) /*200 + rand.Next(-120, 80)*/ };
                bars[i].Label = bars[i].Value.ToString();

                if (bars[i].Value >= 190)
                {
                    bars[i].FillColor = ScottPlot.Colors.Green;
                }
                else if (bars[i].Value > 150 && bars[i].Value < 190)
                {
                    bars[i].FillColor = ScottPlot.Colors.Green;
                }
                else
                {
                    bars[i].FillColor = ScottPlot.Colors.Yellow;
                }
                ticks[i] = new Tick(i, produccion.Rows[i]["Hora"].ToString());

            }


            var Barplot = ProduccionPlot.Plot.Add.Bars(bars);
            ProduccionPlot.Plot.HideGrid();
            Barplot.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;

            ProduccionPlot.Plot.Add.Palette = new ScottPlot.Palettes.Penumbra();

            ProduccionPlot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ProduccionPlot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            ProduccionPlot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
            ProduccionPlot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromHex("#404040");
            ProduccionPlot.Plot.Legend.FontColor = ScottPlot.Color.FromHex("#d7d7d7");
            ProduccionPlot.Plot.Legend.OutlineColor = ScottPlot.Color.FromHex("#d7d7d7");

            ProduccionPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            ProduccionPlot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            ProduccionPlot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);


            var line = ProduccionPlot.Plot.Add.Line(-.5, 200, 12.5, 200);
            line.LinePattern = LinePattern.Dashed;


            ProduccionPlot.Plot.Axes.SetLimits(-0.5, 11.5, 0, 300);

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(ProduccionPlot);
            interaction.Disable();
            ProduccionPlot.Interaction = interaction;
        }

        private void TiempoNormal()
        {
            Bar[] bars = new Bar[10];
            Tick[] ticks;
            int j;

            Random rand = new Random();

            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 17)
            {
                produccion = db.Produccion(1);
                j = 10;
                ticks = new Tick[j];
                bars = new Bar[j];
            }
            else
            {
                produccion = db.Produccion(3);
                j = 9;
                ticks = new Tick[j];
                bars = new Bar[j];
            }


            for (int i = 0; i < j; i++)
            {
                //MessageBox.Show(produccion.Rows[i]["Buenas"].ToString() + " " + i.ToString()); ;
                bars[i] = new Bar() { Position = i, Value = int.Parse(produccion.Rows[i]["Buenas"].ToString()) /*200 + rand.Next(-120, 80)*/ };
                bars[i].Label = bars[i].Value.ToString();

                if (bars[i].Value >= 190)
                {
                    bars[i].FillColor = ScottPlot.Colors.Green;
                }
                else if (bars[i].Value > 150 && bars[i].Value < 190)
                {
                    bars[i].FillColor = ScottPlot.Colors.Green;
                }
                else
                {
                    bars[i].FillColor = ScottPlot.Colors.Yellow;
                }
                ticks[i] = new Tick(i, produccion.Rows[i]["Hora"].ToString());

            }


            var Barplot = ProduccionPlot.Plot.Add.Bars(bars);
            ProduccionPlot.Plot.HideGrid();
            Barplot.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;

            ProduccionPlot.Plot.Add.Palette = new ScottPlot.Palettes.Penumbra();

            ProduccionPlot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            ProduccionPlot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            ProduccionPlot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
            ProduccionPlot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromHex("#404040");
            ProduccionPlot.Plot.Legend.FontColor = ScottPlot.Color.FromHex("#d7d7d7");
            ProduccionPlot.Plot.Legend.OutlineColor = ScottPlot.Color.FromHex("#d7d7d7");

            ProduccionPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            ProduccionPlot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            ProduccionPlot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);


            var line = ProduccionPlot.Plot.Add.Line(-.5, 200, 10.5, 200);
            line.LinePattern = LinePattern.Dashed;


            ProduccionPlot.Plot.Axes.SetLimits(-0.5, 10.5, 0, 300);

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(ProduccionPlot);
            interaction.Disable();
            ProduccionPlot.Interaction = interaction;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TE)
            {
                ProduccionPlot.Reset();
                TiempoNormal();
                ProduccionPlot.Refresh();
                TE = false;
            }
            else
            {
                ProduccionPlot.Reset();
                TiempoExtra();
                ProduccionPlot.Refresh();
                TE = true;
            }
        }
    }
}
