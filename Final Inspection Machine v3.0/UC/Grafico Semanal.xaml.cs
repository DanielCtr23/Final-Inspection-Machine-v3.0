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
    /// Lógica de interacción para Grafico_Semanal.xaml
    /// </summary>
    public partial class Grafico_Semanal : UserControl
    {
        DB db = new DB();
        public Grafico_Semanal()
        {
            InitializeComponent();
            Actualizar();
        }
        public void Actualizar()
        {
            DataTable Produccion = db.ProduccionSemanal();

            Bar[] barBuenas = new Bar[7];
            Bar[] barMalas = new Bar[7];
            Tick[] ticks = new Tick[7];

            for (int i = 0; i < 7; i++)
            {
                barBuenas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["Buenas"].ToString()), FillColor = ScottPlot.Colors.Green };
                barBuenas[i].Label = barBuenas[i].Value.ToString();
                barBuenas[i].LabelOffset = -20;
                barMalas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["Malas"].ToString()), FillColor = ScottPlot.Colors.Red };
                barMalas[i].Label = barMalas[i].Value.ToString();
                barMalas[i].LabelOffset = -20;
                barMalas[i].Value = barMalas[i].Value + barBuenas[i].Value;

                ticks[i] = new Tick(i, Produccion.Rows[i]["Dia"].ToString());
            }

            var Barplot = Plot.Plot.Add.Bars(barMalas);
            Plot.Plot.Add.Bars(barBuenas);
            Plot.Plot.HideGrid();
            Barplot.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
            Plot.Plot.Add.Palette = new ScottPlot.Palettes.Penumbra();
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            Plot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            Plot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
            Plot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromHex("#404040");
            Plot.Plot.Legend.FontColor = ScottPlot.Color.FromHex("#d7d7d7");
            Plot.Plot.Legend.OutlineColor = ScottPlot.Color.FromHex("#d7d7d7");



            Plot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            Plot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
            var line = Plot.Plot.Add.Line(-.5, 200, 12.5, 200);
            line.LinePattern = LinePattern.Dashed;


            Plot.Plot.Axes.SetLimits(-0.5, 6.5, 0, 6000);

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

        }
    }
}
