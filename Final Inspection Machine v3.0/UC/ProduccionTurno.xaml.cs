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
        //DB db = new DB();
        //DataTable produccion;
        DataManager DM = new DataManager();
        public ProduccionTurno()
        {
            InitializeComponent();
            Refresh(false);
        }



        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        public void Refresh(bool TE)
        {
            ProduccionPlot.Reset();
            DateTime Inicio, Fin;
            
            if (TE)
            {
                if (DateTime.Now.Hour >=7 && DateTime.Now.Hour < 19)
                {
                    Inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);
                    Fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 59, 59);
                }
                else if (DateTime.Now.Hour >= 19)
                {
                    Inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0);
                    Fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 59, 59).AddDays(1);
                }
                else
                {
                    Inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0).AddDays(-1);
                    Fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 59, 59);
                }
            }
            else
            {
                if (DateTime.Now.Hour >= 7 && (DateTime.Now.Hour <= 15 || (DateTime.Now.Hour == 16 && DateTime.Now.Minute < 36 )))
                {
                    Inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);
                    Fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 35, 59);
                }
                else if ((DateTime.Now.Hour == 16 && DateTime.Now.Minute >= 36) || DateTime.Now.Hour > 16)
                {
                    Inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 36, 0);
                    Fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 30, 00).AddDays(1);
                }
                else
                {
                    Inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 36, 0).AddDays(-1);
                    Fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 30, 00);
                }
            }

            DataTable dt = DM.Produccion(Inicio, Fin, "H");
            int Horas = dt.Rows.Count;

            Bar[] bars = new Bar[Horas];
            Tick[] ticks = new Tick[Horas];

            for (int i = 0; i < Horas; i++)
            {
                bars[i] = new Bar() { Position = i, Value = int.Parse(dt.Rows[i]["OK"].ToString())};
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
                ticks[i] = new Tick(i, dt.Rows[i]["fecha_formateada"].ToString());
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


            var line = ProduccionPlot.Plot.Add.Line(-.5, 200, Horas-.5, 200);
            line.LinePattern = LinePattern.Dashed;


            ProduccionPlot.Plot.Axes.SetLimits(-0.5, Horas-.5, 0, 330);

            ProduccionPlot.Refresh();

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(ProduccionPlot);
            interaction.Disable();
            ProduccionPlot.Interaction = interaction;
        }
    }
}
