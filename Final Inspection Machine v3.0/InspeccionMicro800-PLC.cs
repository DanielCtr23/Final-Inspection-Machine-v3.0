using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionMicro800
    {
        Micro800 Com;
        String Error1;
        String Error2;
        volatile bool Abortar = false;

        private void InicializarPLC()
        {
            Com = new Micro800();
            if (Com.Conexion())
            {
                Com.Terminar();
                Com.CambioModelo += Com_CambioModelo;
                Com.IniciarCiclo += Com_IniciarCiclo;
                Com.InspeccionarEtiqueta += Com_InspeccionarEtiqueta;
                Com.InspeccionarTapon += Com_InspeccionarTapon;
                Com.MensajeRecibido += Com_MensajeRecibido;
                Com.DetenerCiclo += Com_DetenerCiclo;
                Com.CambioSeleccionado += Com_CambioSeleccionado;
            }
        }

        private void Com_CambioSeleccionado(object sender, string e)
        {
            ModeloBtn.Content = e;
        }

        private void Com_DetenerCiclo(object sender, EventArgs e)
        {
            Abortar = true;
            EsperarEtiquetaE1.Set();
            EsperarEtiquetaE2.Set();
            EsperarTaponE1.Set();
            EsperarTaponE2.Set();
        }

        private void Com_MensajeRecibido(object sender, int e)
        {
            if (e == 0)
            {
                MensajeProceso.Text = "MAQUINA LISTA";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e == 1)
            {
                MensajeProceso.Text = "PARO DE EMERGENCIA PRESIONADO";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Yellow;
            }
            else if (e == 2)
            {
                MensajeProceso.Text = "CORTINAS OBSTRUIDAS";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e == 3)
            {
                MensajeProceso.Text = "CICLO EN CURSO";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e == 4)
            {
                MensajeProceso.Text = "CICLO PAUSADO: ESPERANDO TAPÓN";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e == 5)
            {
                MensajeProceso.Text = "CICLO PAUSADO: ESPERANDO ETIQUETA";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e == 6)
            {
                MensajeProceso.Text = "CORTINAS OBSTRUIDAS: ESPERANDO TAPÓN";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e == 7)
            {
                MensajeProceso.Text = "CORTINAS OBSTRUIDAS: ESPERANDO ETIQUETA";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Transparent;
            }
        }

        private void Com_InspeccionarTapon(object sender, EventArgs e)
        {
            EsperarTaponE1.Set();
            EsperarTaponE2.Set();
        }

        private void Com_InspeccionarEtiqueta(object sender, EventArgs e)
        {
            EsperarEtiquetaE1.Set();
            EsperarEtiquetaE2.Set();
        }

        private void Com_IniciarCiclo(object sender, EventArgs e)
        {
            try
            {
                HiloPrincipal = new Thread(Ejecucion);
                HiloPrincipal.IsBackground = true;
                HiloPrincipal.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede iniciar el ciclo: Com_IniciarCiclo\n" + ex.Message);
            }
        }

        private void Com_CambioModelo(object sender, bool e)
        {
            //ModeloBtn.Content = Com.ModeloSeleccionado();
            if (e)
            {
                ModeloBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38, 38, 38));
            }
            else
            {
                ModeloBtn.Background = new SolidColorBrush(Colors.Red);
            }
            pilotbracket = Com.PilotBracket();
            OcultarPilotBracket(true);
            OcultarResorte(false);
        }
        private void EstadoE1(int op)
        {
            if (op == 0)
            {
                MensajeE1.Text = "Ejecutando Pruebas " + Error1;
                MensajeE1.Foreground = Brushes.White;
            }
            if (op == 1)
            {
                MensajeE1.Text = "Pieza OK " + Error1;
                MensajeE1.Foreground = Brushes.Green;
            }
            if (op == 2)
            {
                MensajeE1.Text = "Pieza NOK " + Error1;
                MensajeE1.Foreground = Brushes.Red;
            }
            if (op == 3)
            {
                MensajeE1.Text = "En Espera";
                MensajeE1.Foreground = Brushes.White;
            }
        }

        private void EstadoE2(int op)
        {
            if (op == 0)
            {
                MensajeE2.Text = "Ejecutando Pruebas " + Error2;
                MensajeE2.Foreground = Brushes.White;
            }
            if (op == 1)
            {
                MensajeE2.Text = "Pieza OK " + Error2;
                MensajeE2.Foreground = Brushes.Green;
            }
            if (op == 2)
            {
                MensajeE2.Text = "Pieza NOK " + Error2;
                MensajeE2.Foreground = Brushes.Red;
            }
            if (op == 3)
            {
                MensajeE2.Text = "En Espera";
                MensajeE2.Foreground = Brushes.White;
            }
        }

    }
}
