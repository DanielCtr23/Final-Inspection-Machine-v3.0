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
    /// Lógica de interacción para SeleccionCorrugado.xaml
    /// </summary>
    public partial class SeleccionCorrugado : Page
    {
        BasicIndicator CorrugadoBI = new BasicIndicator();
        BasicIndicator CorrugadoCI = new BasicIndicator();
        BasicIndicator CorrugadoDI = new BasicIndicator();
        BasicIndicator CorrugadoEI = new BasicIndicator();
        BasicIndicator CorrugadoFI = new BasicIndicator();
        BasicIndicator CorrugadoGI = new BasicIndicator();
        public SeleccionCorrugado()
        {
            InitializeComponent();
            InicializarIndicadores();
        }
        private void InicializarIndicadores()
        {

            CorrugadoBI.Text = "B";
            CorrugadoCI.Text = "C";
            CorrugadoDI.Text = "D";
            CorrugadoEI.Text = "E";
            CorrugadoFI.Text = "F";
            CorrugadoGI.Text = "G";

            //try
            //{
            //    CorrugadoBI.PLCAddressSelectColor2 = "Posicion_Corrugado_B";
            //    CorrugadoCI.PLCAddressSelectColor2 = "Posicion_Corrugado_C";
            //    CorrugadoDI.PLCAddressSelectColor2 = "Posicion_Corrugado_D";
            //    CorrugadoEI.PLCAddressSelectColor2 = "Posicion_Corrugado_E";
            //    CorrugadoFI.PLCAddressSelectColor2 = "Posicion_Corrugado_F";
            //    CorrugadoGI.PLCAddressSelectColor2 = "Posicion_Corrugado_G";

            //    CorrugadoBI.PLCAddressSelectColor3 = "PosicionV_Corrugado_B";
            //    CorrugadoCI.PLCAddressSelectColor3 = "PosicionV_Corrugado_C";
            //    CorrugadoDI.PLCAddressSelectColor3 = "PosicionV_Corrugado_D";
            //    CorrugadoEI.PLCAddressSelectColor3 = "PosicionV_Corrugado_E";
            //    CorrugadoFI.PLCAddressSelectColor3 = "PosicionV_Corrugado_F";
            //    CorrugadoGI.PLCAddressSelectColor3 = "PosicionV_Corrugado_G";
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("No se puede conectar con alguno de los Tags, Favor de verificar");
            //}
            CorrugadoBHost.Child = CorrugadoBI;
            CorrugadoCHost.Child = CorrugadoCI;
            CorrugadoDHost.Child = CorrugadoDI;
            CorrugadoEHost.Child = CorrugadoEI;
            CorrugadoFHost.Child = CorrugadoFI;
            CorrugadoGHost.Child = CorrugadoGI;

            CorrugadoBHost.Background = Brushes.Transparent;
            CorrugadoCHost.Background = Brushes.Transparent;
            CorrugadoDHost.Background = Brushes.Transparent;
            CorrugadoEHost.Background = Brushes.Transparent;
            CorrugadoFHost.Background = Brushes.Transparent;
            CorrugadoGHost.Background = Brushes.Transparent;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double H = CorrugadoBI.Height;
            double W = CorrugadoBI.Width;

            if (H > W)
            {
                CorrugadoBI.Height = (int)W;
                CorrugadoBI.Width = (int)W;
                CorrugadoCI.Height = (int)W;
                CorrugadoCI.Width = (int)W;
                CorrugadoDI.Height = (int)W;
                CorrugadoDI.Width = (int)W;
                CorrugadoEI.Height = (int)W;
                CorrugadoEI.Width = (int)W;
                CorrugadoFI.Height = (int)W;
                CorrugadoFI.Width = (int)W;
                CorrugadoGI.Height = (int)W;
                CorrugadoGI.Width = (int)W;
            }
            else
            {
                CorrugadoBI.Height = (int)H;
                CorrugadoBI.Width = (int)H;
                CorrugadoCI.Height = (int)H;
                CorrugadoCI.Width = (int)H;
                CorrugadoDI.Height = (int)H;
                CorrugadoDI.Width = (int)H;
                CorrugadoEI.Height = (int)H;
                CorrugadoEI.Width = (int)H;
                CorrugadoFI.Height = (int)H;
                CorrugadoFI.Width = (int)H;
                CorrugadoGI.Height = (int)H;
                CorrugadoGI.Width = (int)H;
            }
        }

    }
}
