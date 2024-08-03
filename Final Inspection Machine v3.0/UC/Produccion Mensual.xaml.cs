using MaterialDesignThemes.Wpf;
using Mysqlx.Crud;
using Org.BouncyCastle.Crypto.Engines;
using ScottPlot;
using ScottPlot.Control;
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
        ScottPlot.Plottables.Text MyHighlightText = new Text();
        string[] callout = new string[31];
        int d;
        int s;
        private double highlightX = double.NaN;
        private double highlightY = double.NaN;
        private AxisLimits savedLimits;


        public Produccion_Mensual()
        {
            InitializeComponent();
            Actualizar(true);
            
        }

        public void Actualizar(bool op)
        {
            InputBindings customInputBindings = new ScottPlot.Control.InputBindings()
            {
                DragPanButton = ScottPlot.Control.MouseButton.Left,
                DragZoomButton = ScottPlot.Control.MouseButton.Right,
                ClickAutoAxisButton = ScottPlot.Control.MouseButton.Middle,
                ClickContextMenuButton = ScottPlot.Control.MouseButton.Right,
                DoubleClickButton = ScottPlot.Control.MouseButton.Left,
            };
            PlotActions customPlotActions = new PlotActions()
            {
                ZoomIn = StandardActions.ZoomIn,
                ZoomOut = StandardActions.ZoomOut,
                PanUp = StandardActions.PanUp,
                PanDown = StandardActions.PanDown,
                PanLeft = StandardActions.PanLeft,
                PanRight = StandardActions.PanRight,
                DragPan = StandardActions.DragPan,
                DragZoom = StandardActions.DragZoom,
                ToggleBenchmark = StandardActions.ToggleBenchmark,
                AutoScale = StandardActions.AutoScale,
            };

            Interaction interaction = new Interaction(Plot)
            {
                Inputs = customInputBindings,
                Actions = customPlotActions,
            };

            Plot.Interaction = interaction;
            Plot.Interaction.Enable();

            Plot.ContextMenu = null;

            try
            {
                
                savedLimits = Plot.Plot.Axes.GetLimits();
                if (!double.IsNaN(MyHighlightText.Location.X) && !double.IsNaN(MyHighlightText.Location.Y))
                {
                    highlightX = MyHighlightText.Location.X;
                    highlightY = MyHighlightText.Location.Y;
                }
                var left = Plot.Plot.Axes.Left.Min;
                var right = Plot.Plot.Axes.Left.Max;
                var b = Plot.Plot.Axes.Bottom.Min;
                var t = Plot.Plot.Axes.Bottom.Max;
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

                try
                {
                    ScottPlot.AxisRules.MaximumSpan rule1 = new ScottPlot.AxisRules.MaximumSpan(
                    xAxis: Plot.Plot.Axes.Bottom,
                    yAxis: Plot.Plot.Axes.Left,
                    xSpan: 20,
                    ySpan: 7000);


                    ScottPlot.AxisRules.LockedBottom bottom = new ScottPlot.AxisRules.LockedBottom(Plot.Plot.Axes.Left, 0);
                    ScottPlot.AxisRules.LockedTop top = new ScottPlot.AxisRules.LockedTop(Plot.Plot.Axes.Left, 7000);

                    ScottPlot.AxisRules.MaximumBoundary boundary =
                        new ScottPlot.AxisRules.MaximumBoundary(Plot.Plot.Axes.Bottom, Plot.Plot.Axes.Left, limits: new AxisLimits(-.4, d - .6, 0, 7000));

                    Plot.Plot.Axes.Rules.Add(rule1);
                    Plot.Plot.Axes.Rules.Add(bottom);
                    Plot.Plot.Axes.Rules.Add(top);
                    Plot.Plot.Axes.Rules.Add(boundary);
                    if (s != d)
                    {
                        s = d;

                    }
                    MyHighlightText = Plot.Plot.Add.Text("", 0, 0);
                    MyHighlightText.LabelFontColor = ScottPlot.Colors.White;
                    MyHighlightText.LabelAlignment = Alignment.LowerLeft;
                    MyHighlightText.LabelBold = true;
                    MyHighlightText.OffsetX = -10;
                    MyHighlightText.OffsetY = 0;

                    if (!double.IsNaN(highlightX) && !double.IsNaN(highlightY))
                    {
                        MyHighlightText.Location = new Coordinates(highlightX, highlightY);
                        MyHighlightText.IsVisible = true;
                        MyHighlightText.LabelText = GetCalloutText((int)highlightX);
                    }

                    Plot.MouseMove += Plot_MouseMove;
                    Plot.Plot.Axes.SetLimits(savedLimits);
                    //Plot.Plot.Axes.SetLimits(left ,right, b, t);
                    Plot.Refresh();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }


        }

        private string GetCalloutText(int index)
        {
            if (index < d && index >= 0)
            {
                return callout[index];
            }
            return "";
        }

        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            Pixel mousePixel = new Pixel(e.GetPosition(Plot).X, e.GetPosition(Plot).Y);
            Coordinates mouseLocation = Plot.Plot.GetCoordinates(mousePixel);
            DataPoint point = new DataPoint(mouseLocation.X, mouseLocation.Y, 0);
            try
            {
                if ((int)(point.X + .5) >= 0 && (int)(point.X + .5) < d)
                {
                    MyHighlightText.IsVisible = true;
                    MyHighlightText.Location = point.Coordinates;
                    MyHighlightText.LabelText = GetCalloutText((int)(point.X + .5));
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
                MessageBox.Show(ex.Message + " " + ((int)(point.X + .5)).ToString());
            }
        }


        public void OKNOK()
        {
            try
            {

                Plot.Reset();

                CultureInfo spanishCulture = new CultureInfo("es-MX");
                DateTime Inicio; DateTime Fin;

                Inicio = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddHours(+7);
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


                    ticks[i] = new Tick(i, DateTime.Parse(Produccion.Rows[i]["fecha_formateada"].ToString()).ToString("ddd d", spanishCulture).ToUpper());
                    callout[i] = Produccion.Rows[i]["fecha_formateada"].ToString() + "\n";
                    callout[i] = callout[i] + "Buenas:" + Produccion.Rows[i]["OK"].ToString() + "\n";
                    callout[i] = callout[i] + "Buenas:" + Produccion.Rows[i]["NOK"].ToString() + "\n";
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
                Plot.Plot.Axes.Bottom.TickLabelStyle.FontSize = 18;
                Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
                var line = Plot.Plot.Add.Line(-.5, 2150, d + .5, 2150);
                line.LinePattern = LinePattern.Dashed;
                var line2 = Plot.Plot.Add.Line(-.5, 1750, d + .5, 1750);
                line2.LinePattern = LinePattern.Dashed;
                var line3 = Plot.Plot.Add.Line(-.5, 4300, d + .5, 4300);
                line3.LinePattern = LinePattern.Dashed;
                var line4 = Plot.Plot.Add.Line(-.5, 3500, d + .5, 3500);
                line4.LinePattern = LinePattern.Dashed;
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

                Inicio = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddHours(+7);
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
                        ticks[j] = new Tick(j, DateTime.Parse(Produccion.Rows[j * modelos]["Fecha"].ToString()).ToString("ddd dd", spanishCulture).ToUpper());
                    }
                }

                Plot.Plot.HideGrid();

                BarPlot[] barplot = new BarPlot[dias];

                for (int i = 0; i < dias; i++)
                {
                    barplot[i] = Plot.Plot.Add.Bars(Cantidades[i]);
                    barplot[i].ValueLabelStyle.ForeColor = ScottPlot.Colors.White;
                    barplot[i].ValueLabelStyle.FontSize = 18;
                    callout[i] = ticks[i].Label + "\n";
                    for (int j = 0; j < modelos; j++)
                    {
                        callout[i] = callout[i] + Produccion.Rows[j]["Modelo"].ToString() + ":" + int.Parse(Produccion.Rows[(i * modelos) + j]["OK"].ToString()) + "\n";
                    }
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
                Plot.Plot.Axes.Bottom.TickLabelStyle.FontSize = 18;
                Plot.Plot.Axes.Margins(bottom: 0, left: .01, right: .01, top: .1);
                var line = Plot.Plot.Add.Line(-.5, 2150, d + .5, 2150);
                line.LinePattern = LinePattern.Dashed;
                var line2 = Plot.Plot.Add.Line(-.5, 1750, d + .5, 1750);
                line2.LinePattern = LinePattern.Dashed;
                var line3 = Plot.Plot.Add.Line(-.5, 4300, d + .5, 4300);
                line3.LinePattern = LinePattern.Dashed;
                var line4 = Plot.Plot.Add.Line(-.5, 3500, d + .5, 3500);
                line4.LinePattern = LinePattern.Dashed;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Plot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
        }

        private void Plot_MouseDown(object sender, MouseButtonEventArgs e)
        {
           if (e.RightButton == MouseButtonState.Pressed)
            {
                // Cancela el menú contextual del clic derecho
                //e.Handled = false;
            }
        }
    }
}