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
using static IV3_Keyence.Estructuras;
using System.Windows.Threading;
using System.Net;
using IV3_Keyence;

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
        IV3 Orifice21 = new IV3();
        int E1, E2;
        string[] Camara = new string[4];
        string[] IPCamara = new string[4];
        bool IV3op;
        public bool inicializacionExitosa = true;
        public InspeccionMicro800()
        {
            InitializeComponent();
            inicializacionExitosa = false;
        }

        public void Inicializar()
        {
            bool inicializacionExitosa = true;

            try
            {
                InicializarPLC();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo Iniciar PLC");
                inicializacionExitosa = false;
            }

            if (inicializacionExitosa)
            {
                try
                {
                    InicializarCamaras();
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudieron Iniciar Camaras IV3");
                    inicializacionExitosa = false;
                }
            }

            if (!inicializacionExitosa)
            {
                this.Close();
                return;
            }

            // Configuración común que siempre debe ejecutarse
            etiquetadora = new Etiquetadora();
            Segundero.Interval = TimeSpan.FromSeconds(1);
            Segundero.Tick += Segundero_Tick;
            Segundero.Start();
            CargarContadores();
            OcultarPilotBracket(true);
            OcultarResorte(false);
            this.IsVisibleChanged += InspeccionMicro800_IsVisibleChanged;
            E1 = DM.Estacion(1);
            E2 = DM.Estacion(2);
        }

        private void InspeccionMicro800_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void InicializarCamaras()
        {
            IV3op = true;
            try
            {
                IPAddress C1IP = IPAddress.Parse("192.168.1.2");
                Corrugado1.AbrirConexion(C1IP, 1024);
                //IPCamara[0] = C1IP.ToString();
            }
            catch (Exception)
            {
                //IPCamara[0] = "0";
                //IV3op = false;
            }
            try
            {
                IPAddress C2IP = IPAddress.Parse("192.168.1.3");
                Corrugado2.AbrirConexion(C2IP, 8500);
                //IPCamara[1] = C2IP.ToString();
            }
            catch (Exception)
            {
                //IPCamara[1] = "0";
                //IV3op = false;
            }
            try
            {
                IPAddress O11IP = IPAddress.Parse("192.168.1.4");
                Orifice11.AbrirConexion(O11IP, 8500);
                //IPCamara[2] = O11IP.ToString();
            }
            catch (Exception)
            {
                //IPCamara[2] = "0";
                //IV3op = false;
            }
            try
            {
                IPAddress O21IP = IPAddress.Parse("192.168.1.6");
                Orifice21.AbrirConexion(O21IP, 8500);
                //IPCamara[3] = O21IP.ToString();
            }
            catch (Exception)
            {
                //IPCamara[3] = "0";
                //IV3op = false;
            }

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
            this.Hide();
        }
    }
}
