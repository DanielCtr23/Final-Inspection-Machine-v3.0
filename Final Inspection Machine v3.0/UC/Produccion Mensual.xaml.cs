using MaterialDesignThemes.Wpf;
using Org.BouncyCastle.Crypto.Engines;
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
    /// Lógica de interacción para Produccion_Mensual.xaml
    /// </summary>
    public partial class Produccion_Mensual : UserControl
    {

        DataManager DM = new DataManager();
        ScottPlot.Plottables.Text MyHighlightText;
        string[] callout = new string[31];
        int d;
        public Produccion_Mensual()
        {
            InitializeComponent();
            Actualizar(true);
        }

        public void Actualizar(bool op)
        {
            if (op)
            {
                try
                {

                    OKNOK();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    Modelos();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            ScottPlot.AxisRules.MaximumSpan rule1 = new ScottPlot.AxisRules.MaximumSpan(
            xAxis: Plot.Plot.Axes.Bottom,
            yAxis: Plot.Plot.Axes.Left,
            xSpan: 15,
            ySpan: 7000);
            Plot.Plot.Axes.Rules.Add(rule1);


            ScottPlot.AxisRules.LockedBottom bottom = new ScottPlot.AxisRules.LockedBottom(Plot.Plot.Axes.Left, 0);
            ScottPlot.AxisRules.LockedTop top = new ScottPlot.AxisRules.LockedTop(Plot.Plot.Axes.Left, 7000);
            Plot.Plot.Axes.Rules.Add(bottom);
            Plot.Plot.Axes.Rules.Add(top);

            ScottPlot.AxisRules.MaximumBoundary boundary =
                new ScottPlot.AxisRules.MaximumBoundary(Plot.Plot.Axes.Bottom, Plot.Plot.Axes.Left, limits: new AxisLimits(-1, d, 0, 7000));

            Plot.Plot.Axes.Rules.Add(boundary);

            Plot.Refresh();

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

                if ((mouseLocation.X - (int)mouseLocation.X) > -.2 && (int)mouseLocation.X <d-1.5)
                {
                    MyHighlightText.IsVisible = true;
                    MyHighlightText.Location = point.Coordinates;
                    //MessageBox.Show(callout[1]);
                    if (callout[((int)(point.X + .5))] != null)
                    {
                        MyHighlightText.LabelText = callout[((int)(point.X + .5))];
                    }
                    else
                    {
                        MyHighlightText.LabelText = "";
                    }
                    MyHighlightText.LabelFontColor = ScottPlot.Colors.White;
                    MyHighlightText.LabelFontSize = 14;

                    if (((int)(point.X + .5)) < 2)
                    {
                        MyHighlightText.LabelOffsetX = 5;
                    }
                    else if (((int)(point.X + .5)) > 28)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        //public void Actualizar()
        //{
        //    LapsoTB.Text = "01 - "+DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " DE " + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-MX")).ToUpper();
        //    DataTable Produccion = db.ProduccionMensual();

        //    Bar[] barBuenas = new Bar[Produccion.Rows.Count];
        //    Bar[] barMalas = new Bar[Produccion.Rows.Count];
        //    Tick[] ticks = new Tick[Produccion.Rows.Count];

        //    for (int i = 0; i < Produccion.Rows.Count; i++)
        //    {
        //        barBuenas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["Buenas"].ToString()), FillColor = ScottPlot.Colors.Green };
        //        barBuenas[i].Label = barBuenas[i].Value.ToString();
        //        barBuenas[i].LabelOffset = -20;
        //        barMalas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["Malas"].ToString()), FillColor = ScottPlot.Colors.Red };
        //        barMalas[i].Label = barMalas[i].Value.ToString();
        //        barMalas[i].LabelOffset = -20;
        //        barMalas[i].Value = barMalas[i].Value + barBuenas[i].Value;

        //        ticks[i] = new Tick(i, Produccion.Rows[i]["Dia"].ToString());
        //    }

        //    var Barplot = Plot.Plot.Add.Bars(barMalas);
        //    Plot.Plot.Add.Bars(barBuenas);
        //    Plot.Plot.HideGrid();
        //    Barplot.ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
        //    Plot.Plot.Add.Palette = new ScottPlot.Palettes.Penumbra();
        //    Plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
        //    Plot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
        //    Plot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
        //    Plot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromHex("#404040");
        //    Plot.Plot.Legend.FontColor = ScottPlot.Color.FromHex("#d7d7d7");
        //    Plot.Plot.Legend.OutlineColor = ScottPlot.Color.FromHex("#d7d7d7");


        //    Plot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
        //    Plot.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
        //    Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
        //    var line = Plot.Plot.Add.Line(-.5, 200, 12.5, 200);
        //    line.LinePattern = LinePattern.Dashed;

        //    ScottPlot.Control.Interaction interaction = new ScottPlot.Control.Interaction(Plot);
        //    //ScottPlot.AxisRules.LockedVertical rule = new ScottPlot.AxisRules.LockedVertical(Plot.Plot.Axes.Left, 0, 6000);
        //    ////ScottPlot.AxisRules.LockedHorizontal rule1 = new ScottPlot.AxisRules.LockedHorizontal(Plot.Plot.Axes.Bottom, 0, Produccion.Rows.Count);
        //    //Plot.Plot.Axes.Rules.Add(rule);
        //    //Plot.Plot.Axes.Rules.Add(rule1);
        //    ScottPlot.AxisRules.MaximumSpan rule1 = new ScottPlot.AxisRules.MaximumSpan(
        //    xAxis: Plot.Plot.Axes.Bottom,
        //    yAxis: Plot.Plot.Axes.Left,
        //    xSpan: 15,
        //    ySpan: 7000);
        //    Plot.Plot.Axes.Rules.Add(rule1); 
        //    ScottPlot.AxisRules.MinimumSpan rule2 = new ScottPlot.AxisRules.MinimumSpan(
        //    xAxis: Plot.Plot.Axes.Bottom,
        //    yAxis: Plot.Plot.Axes.Left,
        //    xSpan: 10,
        //    ySpan: 4000);
        //    Plot.Plot.Axes.Rules.Add(rule2);

        //    ScottPlot.AxisRules.LockedBottom bottom = new ScottPlot.AxisRules.LockedBottom(Plot.Plot.Axes.Left, 0);
        //    Plot.Plot.Axes.Rules.Add(bottom);

        //    ScottPlot.AxisRules.MaximumBoundary boundary = 
        //        new ScottPlot.AxisRules.MaximumBoundary(Plot.Plot.Axes.Bottom, Plot.Plot.Axes.Left, limits: new AxisLimits(-0.5, Produccion.Rows.Count-0.5, 0, 5000));

        //    Plot.Plot.Axes.Rules.Add(boundary);
        //}

        public void OKNOK()
        {
            try
            {

                Plot.Reset();
                CultureInfo spanishCulture = new CultureInfo("es-MX");
                DateTime Inicio; DateTime Fin;

                Inicio = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                Fin = Inicio.AddMonths(1).AddSeconds(-1);


                DataTable Produccion = DM.Produccion(Inicio, Fin, "D");

                LapsoTB.Text = Inicio.ToString() + " - " + Fin.ToString();

                int dias = Produccion.Rows.Count;
                d = dias;

                Bar[] barBuenas = new Bar[dias];
                Bar[] barMalas = new Bar[dias];
                Tick[] ticks = new Tick[dias];

                for (int i = 0; i < dias; i++)
                {
                    barBuenas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["OK"].ToString()), FillColor = ScottPlot.Colors.Green };
                    barMalas[i] = new Bar() { Position = i, Value = int.Parse(Produccion.Rows[i]["NOK"].ToString()), FillColor = ScottPlot.Colors.Red }; barBuenas[i].Label = barBuenas[i].Value.ToString();
                    barBuenas[i].Label = barBuenas[i].Value.ToString();
                    barMalas[i].Label = barMalas[i].Value.ToString();
                    barBuenas[i].LabelOffset = -25;
                    barMalas[i].LabelOffset = -25;
                    barMalas[i].Value = barBuenas[i].Value + barMalas[i].Value;


                    ticks[i] = new Tick(i, Produccion.Rows[i]["fecha_formateada"].ToString());
                    callout[i] = Produccion.Rows[i]["fecha_formateada"].ToString();
                    //MessageBox.Show(callout[i]);
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
                var line = Plot.Plot.Add.Line(-.5, 200, 12.5, 200);
                line.LinePattern = LinePattern.Dashed;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Modelos()
        {
            try
            {

                Plot.Reset();
                CultureInfo spanishCulture = new CultureInfo("es-MX");
                DateTime Inicio; DateTime Fin;

                Inicio = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                Fin = Inicio.AddMonths(1).AddSeconds(-1);


                DataTable Produccion = DM.ProduccionModelos(Inicio, Fin, "D");

                LapsoTB.Text = Inicio.ToString() + " - " + Fin.ToString();

                int dias = DateTime.DaysInMonth(Inicio.Year, Inicio.Month);
                d = dias;
                int modelos = Produccion.Rows.Count / dias;
                ScottPlot.Palettes.Category10 palette = new ScottPlot.Palettes.Category10();


                Bar[][] Cantidades = new Bar[dias][];
                Tick[] ticks = new Tick[dias];

                for (int i = 0; i < dias; i++)
                {
                    Cantidades[i] = new Bar[modelos];
                }
                int[] acum = new int[dias];
                for (int i = modelos - 1; i >= 0; i--)
                {
                    int k = 0;
                    for (int j = 0; j < dias; j++)
                    {
                        Cantidades[j][i] = new Bar()
                        {
                            Position = j,
                            Value = int.Parse(Produccion.Rows[(j * modelos) + i]["OK"].ToString()) + acum[j],
                            FillColor = palette.GetColor(i)
                        };
                        Cantidades[j][i].Label = (Cantidades[j][i].Value - acum[j]).ToString();
                        Cantidades[j][i].LabelOffset = -25;
                        acum[j] = (int)Cantidades[j][i].Value;
                        ticks[j] = new Tick(j, Produccion.Rows[j * modelos]["Fecha"].ToString());
                    }
                }

                Plot.Plot.HideGrid();

                BarPlot[] barplot = new BarPlot[dias];

                for (int i = 0; i < dias; i++)
                {
                    barplot[i] = Plot.Plot.Add.Bars(Cantidades[i]);
                    barplot[i].ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
                    barplot[i].ValueLabelStyle.FontSize = 18;
                    callout[i] = ticks[i].Label;
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
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
