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
        string[] Camara = new string[4];
        string[] IPCamara = new string[4];
        bool IV3op;
        public InspeccionMicro800()
        {
            InitializeComponent();
            try
            {
                InicializarPLC();
            }
            catch (Exception)
            {

            }
            ShowLoadingAndInitializeAsync();

        }


        private async void ShowLoadingAndInitializeAsync()
        {
            var loading = new LoadingForm();  // Asume que Loading es una ventana o formulario
            loading.Show();

            try
            {
                await Task.Run(() =>
                {

                    // Suponiendo que Loading.Carga1 puede ser llamado desde cualquier hilo
                    // Si no es así, debes invocarlo en el hilo de la interfaz de usuario
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (Com.Conexion())
                        {
                            loading.Carga1("Micro800", Com.Com.IPAddress, "FIM 3 CL", true);
                        }
                        else
                        {
                            loading.Carga1("Micro800", Com.Com.IPAddress, "FIM 3 CL", false);
                        }
                    });


                    Thread.Sleep(500);
                });
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Manejar la excepción y actualizar la UI en el hilo principal
                    loading.Carga1("Micro800", Com.Com.IPAddress, "FIM 3 CL", false);
                });
            }
            finally
            {
                try
                {
                    await Task.Run(() =>
                    {
                        InicializarCamaras();


                        Camara[0] = "Corrugado 1";
                        Camara[1] = "Corrugado 2";
                        Camara[2] = "Orifice 11";
                        Camara[3] = "Orifice 21";

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            loading.Carga2(Camara, IPCamara, IV3op);
                        });


                        Thread.Sleep(500);
                    });
                }
                catch (Exception)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        loading.Carga2(Camara, IPCamara, IV3op);
                    });

                }


                // Cerrar el diálogo de carga en el hilo de la interfaz de usuario
                Application.Current.Dispatcher.Invoke(() =>
                {
                    loading.Close();
                });

                if (!IV3op || !Com.Conexion())
                {
                    this.Close();
                }

            }

            // Continuar con la inicialización del formulario principal
            etiquetadora = new Etiquetadora();
            Segundero.Interval = TimeSpan.FromSeconds(1);
            Segundero.Tick += Segundero_Tick;
            Segundero.Start();
            CargarContadores();
            OcultarPilotBracket(true);
            OcultarResorte(false);
            this.IsVisibleChanged += InspeccionMicro800_IsVisibleChanged; ;
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
                IPCamara[0] = C1IP.ToString();
            }
            catch (Exception)
            {
                IPCamara[0] = "0";
                IV3op = false;
            }
            try
            {
                IPAddress C2IP = IPAddress.Parse("192.168.1.3");
                Corrugado2.AbrirConexion(C2IP, 8500);
                IPCamara[1] = C2IP.ToString();
            }
            catch (Exception)
            {
                IPCamara[1] = "0";
                IV3op = false;
            }
            try
            {
                IPAddress O11IP = IPAddress.Parse("192.168.1.4");
                Orifice11.AbrirConexion(O11IP, 8500);
                IPCamara[2] = O11IP.ToString();
            }
            catch (Exception)
            {
                IPCamara[2] = "0";
                IV3op = false;
            }
            try
            {
                IPAddress O21IP = IPAddress.Parse("192.168.1.6");
                Orifice21.AbrirConexion(O21IP, 8500);
                IPCamara[3] = O21IP.ToString();
            }
            catch (Exception)
            {
                IPCamara[3] = "0";
                IV3op = false;
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
            Form1 form = new Form1(Com.Com);
            form.ShowDialog();
        }

        private void RegresarBtn_Click(object sender, RoutedEventArgs e)
        {   
            this.Hide();
        }
    }
}
