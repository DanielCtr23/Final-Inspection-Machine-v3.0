using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionCL2
    {

        CompactLogix Com;

        private void InicializarPLC()
        {
            Com = new CompactLogix();
            Com.CambioModelo += Com_CambioModelo;
            Com.IniciarCiclo += Com_IniciarCiclo;
            Com.InspeccionarEtiqueta += Com_InspeccionarEtiqueta;
            Com.InspeccionarTapon += Com_InspeccionarTapon;
            Com.MensajeRecibido += Com_MensajeRecibido;
            Com.DetenerCiclo += Com_DetenerCiclo;
        }

        private void Com_DetenerCiclo(object sender, EventArgs e)
        {
        }

        private void Com_MensajeRecibido(object sender, int e)
        {
            if (e==0)
            {
                MensajeProceso.Text = "MAQUINA LISTA";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            if (e==1)
            {
                MensajeProceso.Text = "PARO DE EMERGENCIA PRESIONADO";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Yellow;
            }
            if (e==2)
            {
                MensajeProceso.Text = "CICLO EN CURSO";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            if (e==3)
            {
                MensajeProceso.Text = "CORTINAS OBSTRUIDAS";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Transparent;
            }
            if (e==4)
            {
                MensajeProceso.Text = "SET-UP INCORRECTO";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Yellow;
            }
        }

        private void Com_InspeccionarTapon(object sender, EventArgs e)
        {
        }

        private void Com_InspeccionarEtiqueta(object sender, EventArgs e)
        {
        }

        private void Com_IniciarCiclo(object sender, EventArgs e)
        {
            pilotbracket = Com.PilotBracket();
            nutrojo = Com.NutRojo();
            sinsentido = Com.SinSentido();
            HiloPrincipal = new System.Threading.Thread(Ejecucion);
            HiloPrincipal.Start();
        }

        private void Com_CambioModelo(object sender, bool e)
        {
            ModeloBtn.Content = Com.ModeloSeleccionado();
            if (e)
            {
                ModeloBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38, 38, 38));
            }
            else
            {
                ModeloBtn.Background = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
