using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        InspeccionCL2 inspeccion_CL2;
        InspeccionMicro800 inspeccion_Micro800;
        DashboardTab DashboardTab;
        int plc;
        DataManager DM = new DataManager();
        public Principal()
        {
            InitializeComponent(); 
            try
            {
                plc = DM.TipoPLC();
            }
            catch (Exception)
            {

                throw;
            }
            if (plc == 0)
            {
                try
                {

                    inspeccion_CL2 = new InspeccionCL2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (plc == 1)
            {
                try
                {
                    inspeccion_Micro800 = new InspeccionMicro800();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (plc==0)
            {
                try
                {
                    if (inspeccion_CL2.inicializacionExitosa == false)
                    {
                        inspeccion_CL2.Inicializar();
                        inspeccion_CL2.IsVisibleChanged += Inspeccion_CL2_IsVisibleChanged;
                        inspeccion_CL2.Closed += Inspeccion_CL2_Closed;
                    }
                    inspeccion_CL2.Show();
                    this.Hide();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if(plc ==1)
            {
                try
                {
                    if (inspeccion_Micro800.inicializacionExitosa == false)
                    {
                        inspeccion_Micro800.Inicializar();
                        inspeccion_Micro800.IsVisibleChanged += Inspeccion_Micro800_IsVisibleChanged;
                        inspeccion_Micro800.Closed += Inspeccion_Micro800_Closed;
                    }
                    inspeccion_Micro800.Show();
                    this.Hide();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void Inspeccion_Micro800_Closed(object sender, EventArgs e)
        {
        }

        private void Inspeccion_Micro800_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                this.Show();
            }
            catch (Exception)
            {

            }
        }

        private void Inspeccion_CL2_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                this.Show();
            }
            catch (Exception)
            {

                throw;
            }
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
                if (inspeccion_Micro800 != null)
                {
                    inspeccion_Micro800.Close();
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

        private void RestartBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea reiniciar la aplicación?", "Reiniciar", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                // Obtener el nombre del ejecutable de la aplicación actual
                string applicationPath = Process.GetCurrentProcess().MainModule.FileName;

                // Iniciar un nuevo proceso para la misma aplicación
                Process.Start(applicationPath);

                // Cerrar la aplicación actual
                Application.Current.Shutdown();
            }
        }
    }
}
