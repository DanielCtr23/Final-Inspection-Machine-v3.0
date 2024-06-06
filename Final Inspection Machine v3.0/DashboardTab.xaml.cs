using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Interaction logic for DashboardTab.xaml
    /// </summary>
    public partial class DashboardTab : Window
    {
        public DashboardTab()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {200, 189, 199, 220, 270, 134, 175},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {2, 4, 5, 7, 1, 10, 5},
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };


            Labels = new[] { "7am", "8am", "9am", "10am", "11am", "12am", "1pm", "2pm", };
            Formatter = value => value + " Pcs.";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }

}
