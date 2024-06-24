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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
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

        private void RegresarBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            
        }

        private void ModeloBI_Paint(object sender, PaintEventArgs e)
        {
            ActualizarModelo();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            dB.ResetContadores();
        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            //AjustarControles(this.ActualHeight);
            //this.SizeChanged += Window_SizeChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InicializarPLC();
            ResultadosE1 = new Estructuras.ResultadosCorrugado[8];
            ResultadosE2 = new Estructuras.ResultadosCorrugado[8];
            dB = new DB();
            etiquetadora = new Etiquetadora();
            //InicializarCamaras();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }


        public Inspeccion_CompactLogix()
        {
            InitializeComponent();
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
            //CheckForIllegalCrossThreadCalls = false;
            //Resetprograma();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //AjustarControles(e.NewSize.Height);
        }


        private void AjustarControles(double H)
        {
            //System.Windows.MessageBox.Show(OrificeTxB1.Height.ToString());
            //OrificeBI1.Width = (int)OrificeTxB1.Height;
            //PilotBracketBI1.Size = OrificeBI1.Size;
            //ResorteBI1.Size = OrificeBI1.Size;
            //LargoBI1.Size = OrificeBI1.Size;
            //SentidoBI1.Size = OrificeBI1.Size;
            //NutBI1.Size = OrificeBI1.Size;
            //TaponBI1.Size = OrificeBI1.Size;
            //EtiquetaBI1.Size = OrificeBI1.Size;
           // OrificeBI2.Size = OrificeBI1.Size;
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
                //PilotBracketBI1.Visible = true;
            }
            else
            {
                ProcesoGrid.RowDefinitions[3].Height = new GridLength(0);
                //PilotBracketBI1.Visible= false;
            }

        }

        private void LimpiarEstaciones()
        {
        //    OrificeBI1.SelectColor2 = false;
        //    OrificeBI1.SelectColor3 = false;
        //    LargoBI1.SelectColor2 = false;
        //    LargoBI1.SelectColor3 = false;
        //    SentidoBI1.SelectColor2 = false;
        //    SentidoBI1.SelectColor3 = false;
        //    NutBI1.SelectColor2 = false;
        //    NutBI1.SelectColor3 = false;
        //    TaponBI1.SelectColor2 = false;
        //    TaponBI1.SelectColor3 = false;
        //    EtiquetaBI1.SelectColor2 = false;
        //    EtiquetaBI1.SelectColor3 = false;

            OrificeBI2.SelectColor2 = false;
            OrificeBI2.SelectColor3 = false;
            LargoBI2.SelectColor2 = false;
            LargoBI2.SelectColor3 = false;
            SentidoBI2.SelectColor2 = false;
            SentidoBI2.SelectColor3 = false;
            NutBI2.SelectColor2 = false;
            NutBI2.SelectColor3 = false;
            TaponBI2.SelectColor2 = false;
            TaponBI2.SelectColor3 = false;
            EtiquetaBI2.SelectColor2 = false;
            EtiquetaBI2.SelectColor3 = false;
        }
    }
}
