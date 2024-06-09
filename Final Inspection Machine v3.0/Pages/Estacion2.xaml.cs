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
    /// Lógica de interacción para Estacion2.xaml
    /// </summary>
    public partial class Estacion2 : Page
    {
        BasicIndicator OrificeBI = new BasicIndicator();
        BasicIndicator PilotBracketBI = new BasicIndicator();
        BasicIndicator ResorteBI = new BasicIndicator();
        BasicIndicator LargoCorrugadoBI = new BasicIndicator();
        BasicIndicator SentidoCorrugadoBI = new BasicIndicator();
        BasicIndicator NutBI = new BasicIndicator();
        BasicIndicator TaponBI = new BasicIndicator();
        BasicIndicator EtiquetaBI = new BasicIndicator();

        public Estacion2()
        {
            InitializeComponent();
            OrificeHost.Child = OrificeBI;
            PilotBracketHost.Child = PilotBracketBI;
            ResorteHost.Child = ResorteBI;
            LargoCorrugadoHost.Child = LargoCorrugadoBI;
            SentidoCorrugadoHost.Child = SentidoCorrugadoBI;
            NutCorrugadoHost.Child = NutBI;
            TaponCorrugadoHost.Child = TaponBI;
            EtiquetaCorrugadoHost.Child = EtiquetaBI;
            MostrarResorte(false);
            MostrarPilotBracket(false);
            Ajustar();
        }

        private void MostrarResorte(bool Activar)
        {
            if (Activar)
            {
                ResorteHost.Visibility = Visibility.Visible;
                ResorteTB.Visibility = Visibility.Visible;
            }
            else
            {
                ResorteHost.Visibility = Visibility.Collapsed;
                ResorteTB.Visibility = Visibility.Collapsed;
                gridControls.RowDefinitions[2].Height = new GridLength(0);

            }
        }

        private void MostrarPilotBracket(bool Activar)
        {
            if (Activar)
            {
                PilotBracketHost.Visibility = Visibility.Visible;
                PilotBracketTB.Visibility = Visibility.Visible;
            }
            else
            {
                PilotBracketHost.Visibility = Visibility.Collapsed;
                PilotBracketTB.Visibility = Visibility.Collapsed;
                gridControls.RowDefinitions[1].Height = new GridLength(0);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            double WP = e.NewSize.Width;
            double HP = e.NewSize.Height;
            double Factor = 13;
            try
            {
                OrificeTB.FontSize = HP / Factor;
                PilotBracketTB.FontSize = HP / Factor;
                ResorteTB.FontSize = HP / Factor;
                LargoTB.FontSize = HP / Factor;
                SentidoTB.FontSize = HP / Factor;
                NutTB.FontSize = HP / Factor;
                TaponTB.FontSize = HP / Factor;
                EtiquetaTB.FontSize = HP / Factor;

            }
            catch (Exception)
            {
                OrificeTB.FontSize = 12;
            }
            Ajustar();
        }

        private void Estacion1Page_Loaded(object sender, RoutedEventArgs e)
        {
            Ajustar();
        }

        public void Ajustar()
        {
            int H = (int)OrificeTB.ActualHeight - 4;
            OrificeBI.Width = H;
            ResorteBI.Width = H;
            PilotBracketBI.Width = H;
            LargoCorrugadoBI.Width = H;
            SentidoCorrugadoBI.Width = H;
            NutBI.Width = H;
            TaponBI.Width = H;
            EtiquetaBI.Width = H;
            OrificeBI.Height = H;
            ResorteBI.Height = H;
            PilotBracketBI.Height = H;
            LargoCorrugadoBI.Height = H;
            SentidoCorrugadoBI.Height = H;
            NutBI.Height = H;
            TaponBI.Height = H;
            EtiquetaBI.Height = H;
        }

        private void OrificeTB_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ajustar();
        }
    }
}
