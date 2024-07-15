﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Lógica de interacción para InspeccionCL2.xaml
    /// </summary>
    public partial class InspeccionCL2 : Window
    {
        DispatcherTimer Segundero = new DispatcherTimer();
        public InspeccionCL2()
        {
            InitializeComponent();
            InicializarPLC();
            InicializarCamaras();
            Thread.Sleep(1000);
            etiquetadora = new Etiquetadora();
            Segundero.Interval = TimeSpan.FromSeconds(1);
            Segundero.Tick += Segundero_Tick;
            Segundero.Start();
            CargarContadores();
            OcultarPilotBracket(true);
            OcultarResorte(false);

        }

        private void InicializarCamaras()
        {
            IPAddress C1IP = IPAddress.Parse("192.168.1.2");
            Corrugado1 = new IV3_Keyence.IV3(C1IP, 8500);
            IPAddress C2IP = IPAddress.Parse("192.168.1.3");
            Corrugado2 = new IV3_Keyence.IV3(C2IP, 8500);
            IPAddress O11IP = IPAddress.Parse("192.168.1.4");
            Orifice11 = new IV3_Keyence.IV3(O11IP, 8500);
            IPAddress O21IP = IPAddress.Parse("192.168.1.7");
            Orifice21 = new IV3_Keyence.IV3(O21IP, 8500);
        }

        private void Segundero_Tick(object sender, EventArgs e)
        {
            FechaTB.Text = "     " + DateTime.Now.ToString("d");
            HoraTB.Text = "  " + DateTime.Now.ToString("T");
        }
        private void OcultarPilotBracket(bool op)
        {
            if (!op)
            {
                ProcesoGrid.RowDefinitions[3].Height = new GridLength(0);
            }
            else
            {
                ProcesoGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
            }
        }
        private void OcultarResorte(bool op)
        {
            if (!op)
            {
                ProcesoGrid.RowDefinitions[4].Height = new GridLength(0);
            }
            else
            {
                ProcesoGrid.RowDefinitions[4].Height = new GridLength(1, GridUnitType.Star);
            }
        }
        private void LimpiarPantalla()
        {
            OrificeBI1.Reset();
            OrificeBI2.Reset();
            PilotBracketBI1.Reset();
            PilotBracketBI2.Reset();
            ResorteBI1.Reset();
            ResorteBI2.Reset();
            LargoBI1.Reset();
            LargoBI2.Reset();
            SentidoBI1.Reset();
            SentidoBI2.Reset();
            NutBI1.Reset();
            NutBI2.Reset();
            TaponBI1.Reset();
            TaponBI2.Reset();
            EtiquetaBI1.Reset();
            EtiquetaBI2.Reset();
        }

        //Pruebas
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (ProcesoGrid.RowDefinitions[3].Height.Value == 0)
            //{
            //    ProcesoGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
            //    OrificeBI1.OK(false);
            //}
            //else
            //{
            //    ProcesoGrid.RowDefinitions[3].Height = new GridLength(0);
            //    OrificeBI1.OK(true);
            //}

        }

        /*private string MensajeEstacion(int op)
        {
            string mensaje;
            if (op == 0)
            {
                mensaje = "INICIANDO PRUEBA";
            }
            else if(op == 1)
            {
                mensaje = "REALIZANDO PRUEBA";
            }
            else if(op == 2)
            {
                mensaje = "ESPERANDO COLOCACIÓN DE TAPÓN";
            }
            else if (op == 3)
            {
                mensaje = "IMPRIMIENDO ETIQUETA";
            }
            else if(op == 4)
            {
                mensaje = "ESPERANDO COLOCACIÓN DE ETIQUETA";
            }
            else if (op == 5)
            {
                mensaje = "PIEZA OK";
            }
            else if (op==6)
            {
                mensaje = "PIEZA NOK";
            }
            else
            {
                mensaje = "";
            }

            return mensaje;
        }*/

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            db.ResetContadores();
            CargarContadores();
        }

        private void ModeloBtn_Click(object sender, RoutedEventArgs e)
        {
            form form = new form(Com.Com);
            form.ShowDialog();
        }

        private void RegresarBtn_Click(object sender, RoutedEventArgs e)
        {
            Com.Cerrar();
            Corrugado1.Dispose();
            Corrugado2.Dispose();
            Orifice21.Dispose(); 
            Orifice11.Dispose();
            this.Close();
        }
    }
}
