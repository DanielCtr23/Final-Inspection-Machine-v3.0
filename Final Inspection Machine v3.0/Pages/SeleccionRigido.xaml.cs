using AdvancedHMIControls;
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

namespace Final_Inspection_Machine_v3._0.Pages
{
    /// <summary>
    /// Lógica de interacción para SeleccionRigido.xaml
    /// </summary>
    public partial class SeleccionRigido : Page
    {
        BasicIndicator RigidoBI = new BasicIndicator();
        BasicIndicator RigidoCI = new BasicIndicator();
        BasicIndicator RigidoDI = new BasicIndicator();
        BasicIndicator RigidoEI = new BasicIndicator();
        BasicIndicator RigidoFI = new BasicIndicator();
        BasicIndicator RigidoGI = new BasicIndicator();
        public SeleccionRigido()
        {
            InitializeComponent();
            InicializarIndicadores();
        }

        private void InicializarIndicadores()
        {

            RigidoBI.Text = "B";
            RigidoCI.Text = "C";
            RigidoDI.Text = "D";
            RigidoEI.Text = "E";
            RigidoFI.Text = "F";
            RigidoGI.Text = "G";

            //try
            //{
            //    RigidoBI.PLCAddressSelectColor2 = "Posicion_Rigido_B";
            //    RigidoCI.PLCAddressSelectColor2 = "Posicion_Rigido_C";
            //    RigidoDI.PLCAddressSelectColor2 = "Posicion_Rigido_D";
            //    RigidoEI.PLCAddressSelectColor2 = "Posicion_Rigido_E";
            //    RigidoFI.PLCAddressSelectColor2 = "Posicion_Rigido_F";
            //    RigidoGI.PLCAddressSelectColor2 = "Posicion_Rigido_G";

            //    RigidoBI.PLCAddressSelectColor3 = "PosicionV_Rigido_B";
            //    RigidoCI.PLCAddressSelectColor3 = "PosicionV_Rigido_C";
            //    RigidoDI.PLCAddressSelectColor3 = "PosicionV_Rigido_D";
            //    RigidoEI.PLCAddressSelectColor3 = "PosicionV_Rigido_E";
            //    RigidoFI.PLCAddressSelectColor3 = "PosicionV_Rigido_F";
            //    RigidoGI.PLCAddressSelectColor3 = "PosicionV_Rigido_G";
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("No se puede conectar con alguno de los Tags, Favor de verificar");
            //}
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
