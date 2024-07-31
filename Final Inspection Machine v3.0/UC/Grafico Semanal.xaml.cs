using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        DataManager DM = new DataManager();
        ScottPlot.Plottables.Text MyHighlightText;
        string[] callout = new string[7];
        public Grafico_Semanal()
        {
            InitializeComponent();
            Actualizar(true);
        }

        public void Actualizar(bool op)
        {
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


        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Pixel mousePixel = new Pixel(e.GetPosition(Plot).X, e.GetPosition(Plot).Y);
                Coordinates mouseLocation = Plot.Plot.GetCoordinates(mousePixel);

                DataPoint point = new DataPoint(mouseLocation.X, mouseLocation.Y, 0);

                if ((mouseLocation.X - (int)mouseLocation.X) > -.2 && (int)mouseLocation.X < 23.3)
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
                    else if (((int)(point.X + .5)) > 5)
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


        public void OKNOK()
        {
            Plot.Reset();
            CultureInfo spanishCulture = new CultureInfo("es-MX");
            DateTime Inicio; DateTime Fin;

            if ((int)DateTime.Today.DayOfWeek == 0)
            {
                Inicio = DateTime.Today.AddDays(-6);
            }
            else
            {
                Inicio = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday));
            }

             Fin = Inicio.AddDays(7);


            DataTable Produccion = DM.Produccion(Inicio, Fin, "S");

            LapsoTB.Text = Inicio.ToString() + " - " + Fin.AddSeconds(-1).ToString() ;

            Bar[] barBuenas = new Bar[7];
            Bar[] barMalas = new Bar[7];
            Tick[] ticks = new Tick[7];

            for (int i = 0; i < 7; i++)
            {
                int si;
                if (i == 6)
                {
                    si = 0; 
                }
                else
                {
                    si = i + 1;
                }
                DayOfWeek s = (DayOfWeek)(si);
                barBuenas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["OK"].ToString()), FillColor = ScottPlot.Colors.Green };
                barMalas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["NOK"].ToString()), FillColor = ScottPlot.Colors.Red }; barBuenas[i].Label = barBuenas[i].Value.ToString();
                barBuenas[i].Label = barBuenas[i].Value.ToString();
                barMalas[i].Label = barMalas[i].Value.ToString();
                barBuenas[i].LabelOffset = -22;
                barMalas[i].LabelOffset = -22;
                barMalas[i].Value = barBuenas[i].Value + barMalas[i].Value;

                if (barMalas[i].Value < 300)
                {
                    barMalas[i].LabelOffset = -22 + (17 - (float)(barMalas[i].Value/17));
                }

                if (barBuenas[i].Value < 300)
                {
                    barBuenas[i].LabelOffset = -22 + (17 - (float)(barBuenas[i].Value/17));
                    if (barMalas[i].Value < 300)
                    {
                        barMalas[i].LabelOffset = (barMalas[i].LabelOffset/17) + 17;
                    }
                    else
                    {
                        barMalas[i].LabelOffset = (barMalas[i].LabelOffset/17) + 17 - 22;
                    }

                }

                ticks[i] = new Tick(i, spanishCulture.DateTimeFormat.GetDayName(s).ToUpper() + " " + Produccion.Rows[i]["fecha_formateada"].ToString());


                callout[i] = ticks[i].Label.ToString() + "\n";
                callout[i] = callout[i] + "Buenas:" + Produccion.Rows[i]["OK"].ToString() + "\n";
                callout[i] = callout[i] + "Malas:" + Produccion.Rows[i]["NOK"].ToString() + "\n";
            }


            var Barplot = Plot.Plot.Add.Bars(barMalas);
            var BarPlot2 = Plot.Plot.Add.Bars(barBuenas);
            Plot.Plot.HideGrid();

            Barplot.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
            Barplot.ValueLabelStyle.FontSize = 22;
            BarPlot2.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
            BarPlot2.ValueLabelStyle.FontSize = 22;

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
            var line = Plot.Plot.Add.Line(-.5, 2150, 24.5, 2150);
            line.LinePattern = LinePattern.Dashed;
            var line2 = Plot.Plot.Add.Line(-.5, 1750, 24.5, 1750);
            line2.LinePattern = LinePattern.Dashed;


            Plot.Plot.Axes.SetLimits(-0.5, 6.5, 0, 6000);

            Plot.Refresh();

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

        }

        public void Modelos()
        {
            Plot.Reset();
            CultureInfo spanishCulture = new CultureInfo("es-MX");
            DateTime Inicio;
            DateTime Fin;

            if ((int)DateTime.Today.DayOfWeek == 0)
            {
                Inicio = DateTime.Today.AddDays(-6);
            }
            else
            {
                Inicio = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday));
            }

            Fin = Inicio.AddDays(7);


            DataTable Produccion = DM.ProduccionModelos(Inicio, Fin, "S");

            LapsoTB.Text = Inicio.ToString() + " - " + Fin.AddSeconds(-1).ToString(); 

            int modelos = Produccion.Rows.Count / 7;
            //MessageBox.Show(modelos.ToString()+" "+ Produccion.Rows.Count.ToString());

            ScottPlot.Palettes.Category10 palette = new ScottPlot.Palettes.Category10();



            Bar[][] Cantidades = new Bar[7][];
            Tick[] ticks = new Tick[7];

            for (int i = 0; i < 7; i++)
            {
                Cantidades[i] = new Bar[modelos];
            }
            int[] acum = new int[7];
            for (int i = modelos - 1; i >= 0; i--)
            {
                int k = 0;
                for (int j = 0; j < 7; j++)
                {
                    int si;
                    if (k == 6)
                    {
                        si = 0;
                    }
                    else
                    {
                        si = k + 1;
                    }
                    k++;
                    Cantidades[j][i] = new Bar()
                    {
                        Position = j,
                        Value = int.Parse(Produccion.Rows[(j * modelos) + i]["OK"].ToString()) + acum[j],
                        FillColor = palette.GetColor(i)
                    };
                    DayOfWeek s = (DayOfWeek)(si);
                    Cantidades[j][i].Label = (Cantidades[j][i].Value - acum[j]).ToString();
                    Cantidades[j][i].LabelOffset = -22;
                    acum[j] = (int)Cantidades[j][i].Value;
                    ticks[j] = new Tick(j, spanishCulture.DateTimeFormat.GetDayName(s).ToUpper() + " " + Produccion.Rows[j*modelos]["Fecha"].ToString());
                }
            }

            for (int i = 0; i < 7; i++)
            {
                callout[i] = ticks[i].Label.ToString() + "\n";
                for (int j = 0; j < modelos; j++)
                {
                    callout[i] = callout[i] + Produccion.Rows[j]["Modelo"].ToString() + ":" + Produccion.Rows[(i * modelos) + j]["OK"].ToString() + "\n";
                }
            }

            BarPlot[] barplot = new BarPlot[7];

            for (int i = 0; i < 7; i++)
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
            Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
            var line = Plot.Plot.Add.Line(-.5, 2150, 24.5, 2150);
            line.LinePattern = LinePattern.Dashed;
            var line2 = Plot.Plot.Add.Line(-.5, 1750, 24.5, 1750);
            line2.LinePattern = LinePattern.Dashed;

            Plot.Refresh();

            Plot.Plot.Axes.SetLimits(-0.5, 6.5, 0, 6000);

            ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
            interaction.Disable();
            Plot.Interaction = interaction;

        }
    }
}
