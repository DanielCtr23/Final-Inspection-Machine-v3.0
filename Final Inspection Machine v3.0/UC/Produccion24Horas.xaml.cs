using ScottPlot;
using ScottPlot.TickGenerators.TimeUnits;
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
    /// Interaction logic for Produccion24Horas.xaml
    /// </summary>
    public partial class Produccion24Horas : UserControl
    {
        public Produccion24Horas()
        {
            InitializeComponent();
            Bar[] bars = new Bar[24];
            Tick[] ticks = new Tick[24];

            Random rand = new Random();
           

            for (int i = 0; i < 24; i++)
            {
                bars[i] = new Bar() { Position = i, Value = 200 + rand.Next(-120, 80) };
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
                ticks[i] = new Tick(i, i.ToString()+":00");

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

            var line = ProduccionPlot.Plot.Add.Line(-.5, 200, 23.5, 200);
            line.LinePattern = LinePattern.Dashed;

            

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(ProduccionPlot);
            interaction.Disable();
            ProduccionPlot.Interaction = interaction;


            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 16)
            {
                var hs = ProduccionPlot.Plot.Add.HorizontalSpan(6.5,16.5 );
                hs.LineStyle.Width = 0;
                hs.LineStyle.Color = ScottPlot.Colors.Green;
                hs.LineStyle.Pattern = LinePattern.Dotted;
                hs.FillStyle.Color = ScottPlot.Colors.White.WithAlpha(.2);

                var hse = ProduccionPlot.Plot.Add.HorizontalSpan(16.5, 18.5);
                hse.FillStyle.Color = ScottPlot.Colors.AntiqueWhite.WithAlpha(.2).WithOpacity(.2);
            }
            else if (DateTime.Now.Hour >= 16 && DateTime.Now.Hour < 19)
            {
                var hs = ProduccionPlot.Plot.Add.HorizontalSpan(6.5, 16.5);
                hs.LineStyle.Width = 0;
                hs.LineStyle.Color = ScottPlot.Colors.Green;
                hs.LineStyle.Pattern = LinePattern.Dotted;
                hs.FillStyle.Color = ScottPlot.Colors.White.WithAlpha(.2);
            }

        }
    }
}
