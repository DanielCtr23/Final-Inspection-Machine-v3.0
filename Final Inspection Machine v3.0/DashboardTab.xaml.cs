﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using Final_Inspection_Machine_v3._0.UC;
using Google.Protobuf.WellKnownTypes;
using LiveCharts;
using LiveCharts.Wpf;
using ScottPlot;
using ScottPlot.WPF;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Interaction logic for DashboardTab.xaml
    /// </summary>
    public partial class DashboardTab : Window
    {
        bool TE = false;
        bool OKNOK = true;
        DispatcherTimer Segundero = new DispatcherTimer();
        public DashboardTab()
        {
            InitializeComponent();
            Segundero.Tick += new EventHandler(Segundero_Tick);
            Segundero.Interval = TimeSpan.FromSeconds(1);
            Segundero.Start();
        }

        private void Segundero_Tick(object sender, EventArgs e)
        {
            HoraTB.Text = DateTime.Now.ToString("T");
            FechaTB.Text = DateTime.Now.ToString("d");
            if (DateTime.Now.Second == 59)
            {
                Refresh();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            TE = true;
            Refresh();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            TE = false;
            Refresh();
        }

        private void Viewbox_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void Refresh()
        {
            try
            {
                ProduccionActual.Refresh();
                ProduccionTurno.Refresh(TE);
                ModelosTurno.Refresh(TE);
                GraficaTurno.Refresh(TE);
                TiemposTurno.Refresh(TE);
                GraficoDiario.Actualizar(OKNOK);
                GraficoSemanal.Actualizar(OKNOK);
                GraficoMensual.Actualizar(OKNOK);
                GraficoAnual.Actualizar(OKNOK);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ToggleButtonProduccion_Checked(object sender, RoutedEventArgs e)
        {
            OKNOK = false;
            Refresh();
        }

        private void ToggleButtonProduccion_Unchecked(object sender, RoutedEventArgs e)
        {
            OKNOK = true;
            Refresh();
        }

        private void ScrollViewer_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }

}
