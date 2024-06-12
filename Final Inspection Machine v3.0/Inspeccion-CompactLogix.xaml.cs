using IV3_Keyence;
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
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Interaction logic for Inspeccion_CompactLogix.xaml
    /// </summary>
    public partial class Inspeccion_CompactLogix : Window
    {

        bool[] Fail = new bool[2];
        bool[] Pass = new bool[2];
        bool[] Libre = new bool[2];

        bool pilotbracket, nutrojo, sinsentido, resorte;

        Estructuras.ResultadosCorrugado[] ResultadosE1, ResultadosE2;

        IV3 Corrugado1, Corrugado2, Orifice11, Orifice12, Orifice21, Orifice22;

        public Inspeccion_CompactLogix()
        {
            InitializeComponent();
            InicializarPLC();
            //InicializarCamaras();
            ResultadosE1 = new Estructuras.ResultadosCorrugado[8];
            ResultadosE2 = new Estructuras.ResultadosCorrugado[8];
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustarControles(e.NewSize.Height);
        }


        private void AjustarControles(double H)
        {
            OrificeBI1.Width = OrificeBI1.Height;
            PilotBracketBI1.Size = OrificeBI1.Size;
            ResorteBI1.Size = OrificeBI1.Size;
            LargoBI1.Size = OrificeBI1.Size;
            SentidoBI1.Size = OrificeBI1.Size;
            NutBI1.Size = OrificeBI1.Size;
            TaponBI1.Size = OrificeBI1.Size;
            EtiquetaBI1.Size = OrificeBI1.Size;
            OrificeBI2.Size = OrificeBI1.Size;
            PilotBracketBI2.Size = OrificeBI2.Size;
            ResorteBI2.Size = OrificeBI2.Size;
            LargoBI2.Size = OrificeBI2.Size;
            SentidoBI2.Size = OrificeBI2.Size;
            NutBI2.Size = OrificeBI2.Size;
            TaponBI2.Size = OrificeBI2.Size;
            EtiquetaBI2.Size = OrificeBI2.Size;

            double Factor = 23;

            //MessageBox.Show(OrificeTxB1.FontSize.ToString());

            OrificeTxB1.FontSize = H/Factor;
            OrificeTxB2.FontSize = H / Factor;
            PilotBracketTxB1.FontSize = H / Factor;
            PilotBracketTxB2.FontSize = H / Factor;
            ResorteTxB1.FontSize = H / Factor;

            LargoTxB1.FontSize = H / Factor;

            SentidoTxB1.FontSize = H / Factor;

            NutTxB1.FontSize = H / Factor;

            TaponTxB1.FontSize = H / Factor;

            EtiquetaTxB1.FontSize = H / Factor;
            EtiquetaTxB2.FontSize = H / Factor;

        }

        private void OcultarResorte(bool visibilidad)
        {
            if (visibilidad)
            {
                ProcesoGrid.RowDefinitions[4].Height = ProcesoGrid.RowDefinitions[2].Height;
            }
            else
            {
                ProcesoGrid.RowDefinitions[4].Height = new GridLength(0);
            }

        }
        private void OcultarPilotBracket(bool visibilidad)
        {
            if (visibilidad)
            {
                ProcesoGrid.RowDefinitions[3].Height = ProcesoGrid.RowDefinitions[2].Height;
            }
            else
            {
                ProcesoGrid.RowDefinitions[3].Height = new GridLength(0);
            }

        }
    }
}
