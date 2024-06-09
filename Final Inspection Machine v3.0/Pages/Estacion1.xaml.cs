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
    /// Interaction logic for Estacion1.xaml
    /// </summary>
    public partial class Estacion1 : Page
    {
        BasicIndicator OrificeBI = new BasicIndicator();
        BasicIndicator PilotBracketBI = new BasicIndicator();
        BasicIndicator ResorteBI = new BasicIndicator();
        BasicIndicator LargoCorrugadoBI = new BasicIndicator();
        BasicIndicator SentidoCorrugadoBI = new BasicIndicator();
        BasicIndicator NutBI = new BasicIndicator();
        BasicIndicator TaponBI = new BasicIndicator();
        BasicIndicator EtiquetaBI = new BasicIndicator();
        public Estacion1()
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
            MostrarPilotBracket(true);
            Ajustar();
        }

        private void MostrarResorte(bool Activar)
        {
            if(Activar)
            {
                ResorteHost.Visibility = Visibility.Visible;
                ResorteTB.Visibility = Visibility.Visible;
            }
            else
            {
                ResorteHost.Visibility = Visibility.Hidden;
                ResorteTB.Visibility = Visibility.Hidden;
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
                PilotBracketHost.Visibility = Visibility.Hidden;
                PilotBracketTB.Visibility = Visibility.Hidden;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            double WP = e.NewSize.Width;
            double HP = e.NewSize.Height;
            try
            {
                OrificeTB.FontSize = HP / 18.75;
                PilotBracketTB.FontSize = HP / 18.75;
                ResorteTB.FontSize = HP / 18.75;
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

        private void Ajustar()
        {
            int H = OrificeBI.Height;
            int W = OrificeBI.Width;


            if (H > W)
            {
                OrificeBI.Height = W;
                ResorteBI.Height = W;
                PilotBracketBI.Height = W;
                LargoCorrugadoBI.Height = W;
                SentidoCorrugadoBI.Height = W;
                NutBI.Height = W;
                TaponBI.Height = W;
                EtiquetaBI.Height = W;
            }
            else
            {
                OrificeBI.Width = H;
                ResorteBI.Width = H;
                PilotBracketBI.Width = H;
                LargoCorrugadoBI.Width = H;
                SentidoCorrugadoBI.Width = H;
                NutBI.Width = H;
                TaponBI.Width = H;
                EtiquetaBI.Width = H;
            }
            OrificeHost.UpdateLayout();
            ResorteHost.UpdateLayout();
            PilotBracketHost.UpdateLayout();
            LargoCorrugadoHost.UpdateLayout();
            SentidoCorrugadoHost.UpdateLayout();
            NutCorrugadoHost.UpdateLayout();
            TaponCorrugadoHost.UpdateLayout();
            EtiquetaCorrugadoHost.UpdateLayout();

            this.UpdateLayout();
        }
    }
}
