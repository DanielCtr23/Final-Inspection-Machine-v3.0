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
        InspeccionMicro800 inspeccion_CL2;
        DashboardTab DashboardTab;
        public Principal()
        {
            InitializeComponent();
            try
            {
                inspeccion_CL2 = new InspeccionMicro800();
                inspeccion_CL2.IsVisibleChanged += Inspeccion_CL2_IsVisibleChanged;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (inspeccion_CL2 != null)
            {
                inspeccion_CL2.Show();
                this.Hide();
            }
            else
            {
                try
                {
                    inspeccion_CL2 = new InspeccionMicro800();
                    inspeccion_CL2.IsVisibleChanged += Inspeccion_CL2_IsVisibleChanged;
                    inspeccion_CL2.Show();
                    this.Hide();
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudo instanciar al ventana de inspección,  revisar conexión de PLC o Tipo de PLC");
                }
            }
        }

        private void Inspeccion_CL2_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Show();
        }

        private void Inspeccion_CL2_Closed(object sender, System.EventArgs e)
        {
            try
            {
                
                this.Show();
            }
            catch (Exception)
            {

            }
        }

        private void DashboardTab_Closed(object sender, EventArgs e)
        {
            try
            {
                this.Show();
            }
            catch (Exception)
            {

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inspeccion_CL2 != null)
                {
                    inspeccion_CL2.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            App.Current.Shutdown();
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DashboardTab = new DashboardTab();
            DashboardTab.Closed += DashboardTab_Closed;
            DashboardTab.Show();
            this.Hide();
        }
    }
}
