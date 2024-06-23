using SciChart.Data.Model;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Interaction logic for ModelosCorridos.xaml
    /// </summary>
    public partial class ModelosCorridos : System.Windows.Controls.UserControl
    {
        DB db = new DB();
        public ModelosCorridos()
        {

            InitializeComponent();
            DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 59, 59)).Tables[0].DefaultView;
            
        }

        public void Refresh(bool TE)
        {
            if (TE)
            {
                TiempoExtra();
            }
            else
            {
                TiempoNormal();
            }
        }

        private void TiempoNormal()
        {
            if ((DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 16) || (DateTime.Now.Hour == 16 && DateTime.Now.Minute < 37))
            {
                DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 36, 59)).Tables[0].DefaultView;
            }
            else if ((DateTime.Now.Hour >= 5 && DateTime.Now.Hour <= 23) || (DateTime.Now.Hour == 16 && DateTime.Now.Minute >= 37))
            {
                DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 37, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 01, 00, 00)).Tables[0].DefaultView;
            }
            else
            {
                DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 16, 37, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 01, 00, 00)).Tables[0].DefaultView;
            }
        }

        private void TiempoExtra()
        {
            if ((DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19))
            {
                DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 59, 59)).Tables[0].DefaultView;
            }
            else if ((DateTime.Now.Hour >= 19 && DateTime.Now.Hour < 24))
            {
                DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 6, 59, 59)).Tables[0].DefaultView;
            }
            else
            {
                DG.ItemsSource = db.ModelosProducidos(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 19, 0, 0),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 59, 59)).Tables[0].DefaultView;
            }
        }

        
    }
}
