using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionCL2
    {

        CompactLogix Com;

        private void InicializarPLC()
        {
            Com = new CompactLogix(1);
            Com.CambioModelo += Com_CambioModelo;
            Com.IniciarCiclo += Com_IniciarCiclo;
            Com.InspeccionarEtiqueta += Com_InspeccionarEtiqueta;
            Com.InspeccionarTapon += Com_InspeccionarTapon;
            Com.MensajeRecibido += Com_MensajeRecibido;
            Com.DetenerCiclo += Com_DetenerCiclo;
            
        }

        private void Com_DetenerCiclo(object sender, EventArgs e)
        {
            try
            {
                if (Estacion1 != null && Estacion1.IsAlive)
                {
                    Estacion1.Abort();
                }

                if (Estacion2 != null && Estacion2.IsAlive)
                {
                    Estacion2.Abort();
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Com.Terminar();
                Thread.Sleep(800);
                ModeloBtn.IsEnabled = true;
                RegresarBtn.IsEnabled = true;
                LimpiarPantalla();
            }
        }

        private void Com_MensajeRecibido(object sender, int e)
        {
            if (e==0)
            {
                MensajeProceso.Text = "MAQUINA LISTA";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e==1)
            {
                MensajeProceso.Text = "PARO DE EMERGENCIA PRESIONADO";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Yellow;
            }
            else if (e==2)
            {
                MensajeProceso.Text = "CORTINAS OBSTRUIDAS";
                MensajeProceso.Foreground = Brushes.Red;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e==3)
            {
                MensajeProceso.Text = "CICLO EN CURSO";
                MensajeProceso.Foreground = Brushes.Green;
                MensajeProceso.Background = Brushes.Transparent;
            }
            else if (e==4)
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
            nutrojo = Com.NutRojo();
            sinsentido = Com.SinSentido();
            HiloPrincipal = new Thread(Ejecucion);
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
            pilotbracket = Com.PilotBracket();
            OcultarPilotBracket(pilotbracket);
            OcultarResorte(resorte);
        }
    }
}
