﻿using System;
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
        //InspeccionCL2 inspeccion_CL2;
        //InspeccionMicro800 inspeccion_Micro800;
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
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (plc==0)
            {
                try
                {
                    InspeccionCL2 inspeccion_CL2 = new InspeccionCL2();
                    inspeccion_CL2.Closed += Inspeccion_CL2_Closed;
                    inspeccion_CL2.Show();
                    this.Close();
                }
                catch (Exception)
                {

                }
            }
            else if(plc ==1)
            {
                try
                {
                    InspeccionMicro800 inspeccion_Micro800 = new InspeccionMicro800();
                    inspeccion_Micro800.Show();
                    inspeccion_Micro800.Closed += Inspeccion_Micro800_Closed;
                    this.Close();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
        }
    }
}
