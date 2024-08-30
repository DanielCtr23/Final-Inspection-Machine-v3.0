using System;
using System.Collections.Generic;
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
using System.Net;
using K_IV3;
using System.Diagnostics;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Lógica de interacción para InspeccionMicro800.xaml
    /// </summary>
    public partial class InspeccionMicro800 : Window
    {
        DispatcherTimer Segundero = new DispatcherTimer();

        IV3 Corrugado1 = new IV3();
        IV3 Corrugado2 = new IV3();
        IV3 Orifice11 = new IV3();
        IV3 Orifice12 = new IV3();
        IV3 Orifice21 = new IV3();
        IV3 Orifice22 = new IV3();

        int E1, E2;
        public InspeccionMicro800()
        {
            InitializeComponent();
            Inicializar();
        }

        public void Inicializar()
        {
            try
            {
                if (!InicializarPLC())
                {
                    MessageBox.Show("No se pudo Iniciar PLC");
                    this.Close();
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo Iniciar PLC");
                this.Close();
                return;
            }

            try
            {
                InicializarCamaras();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudieron Iniciar Camaras IV3");
                this.Close();
                return;
            }


            // Configuración común que siempre debe ejecutarse\
            Segundero.Interval = TimeSpan.FromSeconds(1);
            Segundero.Tick += Segundero_Tick;
            Segundero.Start();
            CargarContadores();
            OcultarPilotBracket(true);
            OcultarResorte(false);
            E1 = DM.Estacion(1);
            E2 = DM.Estacion(2);
        }


        private bool InicializarCamaras()
        {
            try
            {
                IPAddress C1IP = IPAddress.Parse("192.168.1.2");
                Corrugado1.connect(C1IP, 8500);
            }
            catch (Exception)
            {
                return false;
            }
            try
            {
                IPAddress C2IP = IPAddress.Parse("192.168.1.3");
                Corrugado2.connect(C2IP, 8500);
            }
            catch (Exception)
            {
                return false;
            }
            try
            {
                IPAddress O11IP = IPAddress.Parse("192.168.1.4");
                Orifice11.connect(O11IP, 8500);
            }
            catch (Exception)
            {
                return false;
            }
            try
            {
                IPAddress O21IP = IPAddress.Parse("192.168.1.6");
                Orifice21.connect(O21IP, 8500);
            }
            catch (Exception)
            {
                return false;
            }
            if (false)
            {
                try
                {
                    IPAddress O12IP = IPAddress.Parse("192.168.1.5");
                    Orifice12.connect(O12IP, 8500);
                }
                catch (Exception)
                {
                    return false;
                }
                try
                {
                    IPAddress O22IP = IPAddress.Parse("192.168.1.6");
                    Orifice22.connect(O22IP, 8500);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;

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

        }


        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            DM.ResetContadores();
            CargarContadores();
        }

        private void ModeloBtn_Click(object sender, RoutedEventArgs e)
        {
            Com.IniciarCiclo -= Com_IniciarCiclo;
            HabilitarBotones(false);
            Form1 form = new Form1(Com.Com);
            form.FormClosed += Form_FormClosed;
            form.ShowDialog();
        }

        private void Form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Com.IniciarCiclo += Com_IniciarCiclo;
            HabilitarBotones(true);
            Com.Terminar();
        }

        private void RegresarBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Com.Cerrar();
                Corrugado1.disconect();
                Corrugado2.disconect();
                Orifice11.disconect();
                ///Orifice12.disconect();
                Orifice21.disconect();
                ///Orifice22.disconect();
                Thread.Sleep(500);
            }
            catch (Exception)
            {
            }
            // Obtener el nombre del ejecutable de la aplicación actual
            //string applicationPath = Process.GetCurrentProcess().MainModule.FileName;

            //// Iniciar un nuevo proceso para la misma aplicación
            //Process.Start(applicationPath);

            //// Cerrar la aplicación actual
            //Application.Current.Shutdown();
            //this.Close();

            Principal p = new Principal();
            p.Show();
            this.Close();
        }
    }
}
