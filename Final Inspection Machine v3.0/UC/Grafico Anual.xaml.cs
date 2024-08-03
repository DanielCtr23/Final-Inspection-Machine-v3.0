using ScottPlot.Plottables;
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
using System.Globalization;

namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Lógica de interacción para Grafico_Anual.xaml
    /// </summary>
    public partial class Grafico_Anual : UserControl
    {
        DataManager DM = new DataManager();
        ScottPlot.Plottables.Text MyHighlightText;
        string[] callout = new string[12];

        public Grafico_Anual()
        {
            InitializeComponent();
            Actualizar(true);
        }
        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Pixel mousePixel = new Pixel(e.GetPosition(Plot).X, e.GetPosition(Plot).Y);
                Coordinates mouseLocation = Plot.Plot.GetCoordinates(mousePixel);

                DataPoint point = new DataPoint(mouseLocation.X, mouseLocation.Y, 0);

                if (((int)(point.X + .5)) >= 0 && ((int)(point.X + .5)) < 12)
                {
                    MyHighlightText.IsVisible = true;
                    MyHighlightText.Location = point.Coordinates;
                    MyHighlightText.LabelText = callout[((int)(point.X + .5))];
                    MyHighlightText.LabelFontColor = ScottPlot.Colors.White;
                    MyHighlightText.LabelFontSize = 14;

                    if (((int)(point.X + .5)) < 2)
                    {
                        MyHighlightText.LabelOffsetX = 5;
                    }
                    else if (((int)(point.X + .5)) > 10)
                    {
                        MyHighlightText.LabelOffsetX = -30;
                    }

                    Plot.Refresh();

                }
                else
                {
                    MyHighlightText.IsVisible = false;
                    Plot.Refresh();
                }
            }
            catch (Exception)
            {

            }



        }

        public void Actualizar(bool op)
        {
            Plot.MouseMove -= Plot_MouseMove;
            if (op)
            {
                OKNOK();
            }
            else
            {
                Modelos();
            }
            MyHighlightText = Plot.Plot.Add.Text("", 0, 0);
            MyHighlightText.LabelAlignment = Alignment.LowerLeft;
            MyHighlightText.LabelBold = true;
            MyHighlightText.OffsetX = -10;
            MyHighlightText.OffsetY = 0;
            Plot.MouseMove += Plot_MouseMove;
        }

        public void OKNOK()
        {
            Plot.Reset();
            Bar[] barBuenas = new Bar[12];
            Bar[] barMalas = new Bar[12];
            Tick[] ticks = new Tick[12];
            DateTime Inicio;
            DateTime Fin;

            Inicio = DateTime.Today.AddDays(-DateTime.Now.DayOfYear+1).AddHours(7);
            Fin = Inicio.AddYears(1);

            LapsoTB.Text = Inicio.ToString() + " - " + Fin.ToString();

            DataTable Produccion = DM.Produccion(Inicio, Fin, "M");

            for (int i = 0; i < 12; i++)
            {
                barBuenas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["OK"].ToString()), FillColor = ScottPlot.Colors.Green };
                barMalas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["NOK"].ToString()), FillColor = ScottPlot.Colors.Red };
                barBuenas[i].Label = barBuenas[i].Value.ToString();
                barMalas[i].Label = barMalas[i].Value.ToString();
                barBuenas[i].LabelOffset = -22;
                barMalas[i].LabelOffset = -22;
                barMalas[i].Value = barBuenas[i].Value + barMalas[i].Value;

                ticks[i] = new Tick(i, DateTime.Parse(Produccion.Rows[i]["fecha_formateada"].ToString()).ToString("MMMM", new CultureInfo("es-MX")).ToUpper());

                callout[i] = DateTime.Parse(Produccion.Rows[i]["fecha_formateada"].ToString()).ToString("MMMM yyyy", new CultureInfo("es-MX")).ToUpper() + "\n";
                callout[i] = callout[i] + "Buenas:" + Produccion.Rows[i]["OK"].ToString() + "\n";
                callout[i] = callout[i] + "Malas:" + Produccion.Rows[i]["NOK"].ToString() + "\n";
            }


            var Barplot = Plot.Plot.Add.Bars(barMalas);
            var Barplot2 = Plot.Plot.Add.Bars(barBuenas);

            Plot.Plot.HideGrid();
            Barplot.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
            Barplot.ValueLabelStyle.FontSize = 18;
            Barplot2.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
            Barplot2.ValueLabelStyle.FontSize = 18;
            Plot.Plot.Add.Palette = new ScottPlot.Palettes.Penumbra();
            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            Plot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            Plot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
            Plot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromHex("#404040");
            Plot.Plot.Legend.FontColor = ScottPlot.Color.FromHex("#d7d7d7");
            Plot.Plot.Legend.OutlineColor = ScottPlot.Color.FromHex("#d7d7d7");



            Plot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            Plot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            Plot.Plot.Axes.Bottom.TickLabelStyle.FontSize = 14;
            Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
            var line = Plot.Plot.Add.Line(-.5, 94500, 11.5, 94500);
            line.LinePattern = LinePattern.Dashed;

            Plot.Refresh();

            Plot.Plot.Axes.SetLimits(-0.5, 11.5, 0, 140000);

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;
        }

        public void Modelos()
        {
            Plot.Reset();
            DateTime Inicio;
            DateTime Fin;


            Inicio = DateTime.Today.AddDays(-DateTime.Now.DayOfYear+1).AddHours(7);
            Fin = Inicio.AddYears(1);


            LapsoTB.Text = Inicio.ToString() + " - " + Fin.ToString();

            DataTable Produccion = DM.ProduccionModelos(Inicio, Fin, "M");

            int modelos = Produccion.Rows.Count / 12;
            //MessageBox.Show(modelos.ToString()+" "+ Produccion.Rows.Count.ToString());

            ScottPlot.Palettes.Category10 palette = new ScottPlot.Palettes.Category10();



            Bar[][] Cantidades = new Bar[12][];
            Tick[] ticks = new Tick[12];

            for (int i = 0; i < 12; i++)
            {
                Cantidades[i] = new Bar[modelos];
            }
            int[] acum = new int[12];
            for (int i = modelos - 1; i >= 0; i--)
            {

                for (int j = 0; j < 12; j++)
                {
                    Cantidades[j][i] = new Bar()
                    {
                        Position = j,
                        Value = int.Parse(Produccion.Rows[(j * modelos) + i]["OK"].ToString()) + acum[j],
                        FillColor = palette.GetColor(i)
                    };
                    Cantidades[j][i].Label = (Cantidades[j][i].Value - acum[j]).ToString();
                    Cantidades[j][i].LabelOffset = -22;
                    acum[j] = (int)Cantidades[j][i].Value;
                    ticks[j] = new Tick(j, DateTime.Parse(Produccion.Rows[(j * modelos)]["Fecha"].ToString()).ToString("MMMM", new CultureInfo("es-MX")).ToUpper());

                }
            }

            for (int i = 0; i < 12; i++)
            {
                callout[i] = DateTime.Parse(Produccion.Rows[(i * modelos)]["Fecha"].ToString()).ToString("MMMM yyyy", new CultureInfo("es-MX")).ToUpper() + "\n";
                for (int j = 0; j < modelos; j++)
                {
                    callout[i] = callout[i] + Produccion.Rows[j]["Modelo"].ToString() + ":" + Produccion.Rows[(i * modelos) + j]["OK"].ToString() + "\n";
                }
            }


            BarPlot[] barplot = new BarPlot[12];

            for (int i = 0; i < 12; i++)
            {
                barplot[i] = Plot.Plot.Add.Bars(Cantidades[i]);
                barplot[i].ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
                barplot[i].ValueLabelStyle.FontSize = 18;
            }

            for (int i = 0; i < modelos; i++)
            {
                LegendItem item = new LegendItem()
                {
                    LabelText = Produccion.Rows[i]["Modelo"].ToString(),
                    FillColor = palette.GetColor(i)
                };
                Plot.Plot.Legend.ManualItems.Add(item);

            }



            Plot.Plot.Legend.Orientation = ScottPlot.Orientation.Horizontal;
            Plot.Plot.ShowLegend(Alignment.UpperRight);

            Plot.Plot.HideGrid();

            Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            Plot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            Plot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
            Plot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromHex("#404040");
            Plot.Plot.Legend.FontColor = ScottPlot.Color.FromHex("#d7d7d7");
            Plot.Plot.Legend.OutlineColor = ScottPlot.Color.FromHex("#d7d7d7");



            Plot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            Plot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            Plot.Plot.Axes.Bottom.TickLabelStyle.FontSize = 14;
            Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
            var line = Plot.Plot.Add.Line(-.5, 94500, 11.5, 94500);
            line.LinePattern = LinePattern.Dashed;

            Plot.Refresh();

            Plot.Plot.Axes.SetLimits(-0.5, 11.5, 0, 140000);

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

        }
    }
}
