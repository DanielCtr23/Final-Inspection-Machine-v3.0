using AdvancedHMIControls;
using AdvancedHMIDrivers;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Final_Inspection_Machine_v3._0.Pages
{
    /// <summary>
    /// Lógica de interacción para SeleccionRigido.xaml
    /// </summary>
    public partial class SeleccionRigido : Page
    {
        DispatcherTimer TmrSegundero = new DispatcherTimer();
        EthernetIPforCLXCom Com;
        BasicIndicator RigidoBI = new BasicIndicator();
        BasicIndicator RigidoCI = new BasicIndicator();
        BasicIndicator RigidoDI = new BasicIndicator();
        BasicIndicator RigidoEI = new BasicIndicator();
        BasicIndicator RigidoFI = new BasicIndicator();
        BasicIndicator RigidoGI = new BasicIndicator();
        public bool s;

        public SeleccionRigido(EthernetIPforCLXCom ClCom)
        {
            Com = ClCom;
            InitializeComponent();
            InicializarIndicadores();
            TmrSegundero.Tick += TmrSegundero_Tick;
            TmrSegundero.Interval = TimeSpan.FromSeconds(10);
            TmrSegundero.Start();
        }

        private void TmrSegundero_Tick(object sender, EventArgs e)
        {
            //Revisar todo
            if(RigidoBI.SelectColor2 && RigidoBI.SelectColor3)
            {
                RigidoBI.Color2 = System.Drawing.Color.Green;
                s = true;
            }
            else
            {
                RigidoBI.Color2 = System.Drawing.Color.Yellow;
                s = false;
            }

            if (RigidoCI.SelectColor2 && RigidoCI.SelectColor3)
            {
                RigidoCI.Color2 = System.Drawing.Color.Green;
                s = true;
            }
            else
            {
                RigidoCI.Color2 = System.Drawing.Color.Yellow;
                s = false;
            }

            if (RigidoDI.SelectColor2 && RigidoDI.SelectColor3)
            {
                RigidoDI.Color2 = System.Drawing.Color.Green;
                s = true;
            }
            else
            {
                RigidoDI.Color2 = System.Drawing.Color.Yellow;
                s = false;
            }

            if (RigidoEI.SelectColor2 && RigidoEI.SelectColor3)
            {
                RigidoEI.Color2 = System.Drawing.Color.Green;
                s = true;
            }
            else
            {
                RigidoEI.Color2 = System.Drawing.Color.Yellow;
                s = false;
            }

            if (RigidoFI.SelectColor2 && RigidoFI.SelectColor3)
            {
                RigidoFI.Color2 = System.Drawing.Color.Green;
                s = true;
            }
            else
            {
                RigidoFI.Color2 = System.Drawing.Color.Yellow;
                s = false;
            }

            if (RigidoGI.SelectColor2 && RigidoGI.SelectColor3)
            {
                RigidoGI.Color2 = System.Drawing.Color.Green;
                s = true;
            }
            else
            {
                RigidoGI.Color2 = System.Drawing.Color.Yellow;
                s = false;
            }

        }

        private void InicializarIndicadores()
        {
            RigidoBI.ComComponent = Com;
            RigidoCI.ComComponent = Com;
            RigidoDI.ComComponent = Com;
            RigidoEI.ComComponent = Com;
            RigidoFI.ComComponent = Com;
            RigidoGI.ComComponent = Com;

            RigidoBI.Text = "B";
            RigidoCI.Text = "C";
            RigidoDI.Text = "D";
            RigidoEI.Text = "E";
            RigidoFI.Text = "F";
            RigidoGI.Text = "G";

            try
            {
                RigidoBI.PLCAddressSelectColor2 = "POSICION_RIGIDO_B";
                RigidoCI.PLCAddressSelectColor2 = "POSICION_RIGIDO_C";
                RigidoDI.PLCAddressSelectColor2 = "POSICION_RIGIDO_D";
                RigidoEI.PLCAddressSelectColor2 = "POSICION_RIGIDO_E";
                RigidoFI.PLCAddressSelectColor2 = "POSICION_RIGIDO_F";
                RigidoGI.PLCAddressSelectColor2 = "POSICION_RIGIDO_G";

                RigidoBI.PLCAddressSelectColor3 = "INPUTS_BITS_2.5";
                RigidoCI.PLCAddressSelectColor3 = "INPUT_BITS.8";
                RigidoDI.PLCAddressSelectColor3 = "INPUT_BITS.7";
                RigidoEI.PLCAddressSelectColor3 = "INPUT_BITS.6";
                RigidoFI.PLCAddressSelectColor3 = "INPUT_BITS.5";
                RigidoGI.PLCAddressSelectColor3 = "INPUT_BITS.4";
            }
            catch (Exception)
            {
                MessageBox.Show("No se puede conectar con alguno de los Tags, Favor de verificar");
            }
            RigidoBHost.Child = RigidoBI;
            RigidoCHost.Child = RigidoCI;
            RigidoDHost.Child = RigidoDI;
            RigidoEHost.Child = RigidoEI;
            RigidoFHost.Child = RigidoFI;
            RigidoGHost.Child = RigidoGI;

            RigidoBHost.Background = Brushes.Transparent;
            RigidoCHost.Background = Brushes.Transparent;
            RigidoDHost.Background = Brushes.Transparent;
            RigidoEHost.Background = Brushes.Transparent;
            RigidoFHost.Background = Brushes.Transparent;
            RigidoGHost.Background = Brushes.Transparent;
        }


        private void Page_LayoutUpdated(object sender, EventArgs e)
        {
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double H = RigidoBI.Height;
            double W = RigidoBI.Width;

            if (H > W)
            {
                RigidoBI.Height = (int)W;
                RigidoBI.Width = (int)W;
                RigidoCI.Height = (int)W;
                RigidoCI.Width = (int)W;
                RigidoDI.Height = (int)W;
                RigidoDI.Width = (int)W;
                RigidoEI.Height = (int)W;
                RigidoEI.Width = (int)W;
                RigidoFI.Height = (int)W;
                RigidoFI.Width = (int)W;
                RigidoGI.Height = (int)W;
                RigidoGI.Width = (int)W;
            }
            else
            {
                RigidoBI.Height = (int)H;
                RigidoBI.Width = (int)H;
                RigidoCI.Height = (int)H;
                RigidoCI.Width = (int)H;
                RigidoDI.Height = (int)H;
                RigidoDI.Width = (int)H;
                RigidoEI.Height = (int)H;
                RigidoEI.Width = (int)H;
                RigidoFI.Height = (int)H;
                RigidoFI.Width = (int)H;
                RigidoGI.Height = (int)H;
                RigidoGI.Width = (int)H;
            }
        }
    }
}
