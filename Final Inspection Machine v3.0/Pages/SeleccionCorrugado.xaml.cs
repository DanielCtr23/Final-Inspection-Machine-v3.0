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
    /// Lógica de interacción para SeleccionCorrugado.xaml
    /// </summary>
    public partial class SeleccionCorrugado : Page
    {
        DispatcherTimer TmrSegundero = new DispatcherTimer();
        EthernetIPforCLXCom Com;
        BasicIndicator CorrugadoBI = new BasicIndicator();
        BasicIndicator CorrugadoCI = new BasicIndicator();
        BasicIndicator CorrugadoDI = new BasicIndicator();
        BasicIndicator CorrugadoEI = new BasicIndicator();
        BasicIndicator CorrugadoFI = new BasicIndicator();
        public SeleccionCorrugado()
        {
            InitializeComponent();
            InicializarIndicadores();
            TmrSegundero.Tick += TmrSegundero_Tick;
            TmrSegundero.Interval = TimeSpan.FromSeconds(10);
            TmrSegundero.Start();
        }


        private void TmrSegundero_Tick(object sender, EventArgs e)
        {
            //Revisar todo
            if (CorrugadoBI.SelectColor2 && CorrugadoBI.SelectColor3)
            {
                CorrugadoBI.Color2 = System.Drawing.Color.Green;
                var s = Parent as Seleccion;
                s.listo[0] = true;
            }
            else
            {
                CorrugadoBI.Color2 = System.Drawing.Color.Yellow;
                var s = Parent as Seleccion;
                s.listo[0] = false;
            }

            if (CorrugadoCI.SelectColor2 && CorrugadoCI.SelectColor3)
            {
                CorrugadoCI.Color2 = System.Drawing.Color.Green;
                var s = Parent as Seleccion;
                s.listo[0] = true;
            }
            else
            {
                CorrugadoCI.Color2 = System.Drawing.Color.Yellow;
                var s = Parent as Seleccion;
                s.listo[0] = false;
            }

            if (CorrugadoDI.SelectColor2 && CorrugadoDI.SelectColor3)
            {
                CorrugadoDI.Color2 = System.Drawing.Color.Green;
                var s = Parent as Seleccion;
                s.listo[0] = true;
            }
            else
            {
                CorrugadoDI.Color2 = System.Drawing.Color.Yellow;
                var s = Parent as Seleccion;
                s.listo[0] = false;
            }

            if (CorrugadoEI.SelectColor2 && CorrugadoEI.SelectColor3)
            {
                CorrugadoEI.Color2 = System.Drawing.Color.Green;
                var s = Parent as Seleccion;
                s.listo[0] = true;
            }
            else
            {
                CorrugadoEI.Color2 = System.Drawing.Color.Yellow;
                var s = Parent as Seleccion;
                s.listo[0] = false;
            }

            if (CorrugadoFI.SelectColor2 && CorrugadoFI.SelectColor3)
            {
                CorrugadoFI.Color2 = System.Drawing.Color.Green;
                var s = Parent as Seleccion;
                s.listo[0] = true;
            }
            else
            {
                CorrugadoFI.Color2 = System.Drawing.Color.Yellow;
                var s = Parent as Seleccion;
                s.listo[0] = false;
            }


        }


        private void InicializarIndicadores()
        {

            CorrugadoBI.Text = "B";
            CorrugadoCI.Text = "C";
            CorrugadoDI.Text = "D";
            CorrugadoEI.Text = "E";
            CorrugadoFI.Text = "F";

            try
            {
                CorrugadoBI.PLCAddressSelectColor2 = "POSICION_CORRUGADO_B";
                CorrugadoCI.PLCAddressSelectColor2 = "POSICION_CORRUGADO_C";
                CorrugadoDI.PLCAddressSelectColor2 = "POSICION_CORRUGADO_D";
                CorrugadoEI.PLCAddressSelectColor2 = "POSICION_CORRUGADO_E";
                CorrugadoFI.PLCAddressSelectColor2 = "POSICION_CORRUGADO_F";

                CorrugadoBI.PLCAddressSelectColor3 = "INPUT_BITS.9";
                CorrugadoCI.PLCAddressSelectColor3 = "INPUT_BITS.10";
                CorrugadoDI.PLCAddressSelectColor3 = "INPUT_BITS.11";
                CorrugadoEI.PLCAddressSelectColor3 = "INPUT_BITS.12";
                CorrugadoFI.PLCAddressSelectColor3 = "INPUT_BITS.13";
            }
            catch (Exception)
            {
                MessageBox.Show("No se puede conectar con alguno de los Tags, Favor de verificar");
            }
            CorrugadoBHost.Child = CorrugadoBI;
            CorrugadoCHost.Child = CorrugadoCI;
            CorrugadoDHost.Child = CorrugadoDI;
            CorrugadoEHost.Child = CorrugadoEI;
            CorrugadoFHost.Child = CorrugadoFI;

            CorrugadoBHost.Background = Brushes.Transparent;
            CorrugadoCHost.Background = Brushes.Transparent;
            CorrugadoDHost.Background = Brushes.Transparent;
            CorrugadoEHost.Background = Brushes.Transparent;
            CorrugadoFHost.Background = Brushes.Transparent;
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
            }
        }

    }
}
