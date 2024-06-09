using Final_Inspection_Machine_v3._0.Pages;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms.Integration;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Interaction logic for Inspeccion_CL.xaml
    /// </summary>
    public partial class Inspeccion_CL : Window
    {
        DispatcherTimer Segundero = new DispatcherTimer();
        public Inspeccion_CL()
        {
            InitializeComponent();
            Segundero.Tick += new EventHandler(Segundero_Tick);
            Segundero.Interval = TimeSpan.FromSeconds(1);
            Segundero.Start(); 
        }

        private void Segundero_Tick(object sender, EventArgs e)
        {
            FechaTB.Text = DateTime.Now.ToString("D");
            HoraTB.Text = DateTime.Now.ToString("T");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var page = ProcesoFrame.Content as DobleEstacion;
            page.Ajustar();
        }

        private void InspeccionWndw_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void InspeccionWndw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void InspeccionWndw_StateChanged(object sender, EventArgs e)
        {
            var page = ProcesoFrame.Content as DobleEstacion;
            page.Ajustar();
        }
    }
}
