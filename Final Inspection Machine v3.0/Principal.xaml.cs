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

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Interaction logic for Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        Inspeccion_CL inspeccion_CL;
        public Principal()
        {
            InitializeComponent();
            inspeccion_CL = new Inspeccion_CL();
            //new Inspeccion_CompactLogix().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            inspeccion_CL.Show();
            inspeccion_CL.Closing += Inspeccion_CL_Closed;

        }

        private void Inspeccion_CL_Closed(object sender, System.EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
