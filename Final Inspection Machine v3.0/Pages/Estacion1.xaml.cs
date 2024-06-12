using AdvancedHMIControls;
using AdvancedHMIDrivers;
//using IV3_Keyence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.Pages
{
    /// <summary>
    /// Interaction logic for Estacion1.xaml
    /// </summary>
    public partial class Estacion1 : Page
    {
        EthernetIPforCLXCom Com;
        //IV3 O1, O2, C1;
        IPAddress IPO1, IPO2, IPC1;
        static ManualResetEvent ME1 = new ManualResetEvent(false);
        static ManualResetEvent ME11 = new ManualResetEvent(false);

        int E1Contador;

        DobleEstacion DobleEstacion;

        bool PBActivo;

        Thread Proceso;
        
        //Traer esto de Fuera
        bool sinsentido, nutrojo, pilotbracket;
        string modelo;


        string Serial1;

        bool E1Pass = false;
        bool E1Fail = false;
        bool E1T;

        Etiquetadora etiquetadora;

        DB dB;

        //Estructuras.ResultadosCorrugado[] ResultadosC1 = new Estructuras.ResultadosCorrugado[5];

        BasicIndicator OrificeBI = new BasicIndicator();
        BasicIndicator PilotBracketBI = new BasicIndicator();
        BasicIndicator ResorteBI = new BasicIndicator();
        BasicIndicator LargoCorrugadoBI = new BasicIndicator();
        BasicIndicator SentidoCorrugadoBI = new BasicIndicator();
        BasicIndicator NutBI = new BasicIndicator();
        BasicIndicator TaponBI = new BasicIndicator();
        BasicIndicator EtiquetaBI = new BasicIndicator();
        public Estacion1(EthernetIPforCLXCom Com)
        {
            InitializeComponent();
            this.Com = Com;
            Hostear();
            MostrarResorte(false);
            ActualizarModelo();
            Ajustar();
            PilotBracketBI.ComComponent = Com;
            PilotBracketBI.PLCAddressSelectColor2 = "PB_E1_OK";
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
            int H = (int)OrificeTB.ActualHeight-4;
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

        private void Hostear()
        {
            OrificeHost.Child = OrificeBI;
            PilotBracketHost.Child = PilotBracketBI;
            ResorteHost.Child = ResorteBI;
            LargoCorrugadoHost.Child = LargoCorrugadoBI;
            SentidoCorrugadoHost.Child = SentidoCorrugadoBI;
            NutCorrugadoHost.Child = NutBI;
            TaponCorrugadoHost.Child = TaponBI;
            EtiquetaCorrugadoHost.Child = EtiquetaBI;
        }

        public void ActualizarModelo()
        {
            if (bool.Parse(Com.Read("PILOT_BRACKET")))
            {
                pilotbracket = true;
            }
            else
            {
                pilotbracket = false;
            }
            MostrarPilotBracket(pilotbracket);

            if(bool.Parse(Com.Read("PROGRAM:FIM.NUT_ROJO")))
            {
                nutrojo = true;
            }
            else
            {
                nutrojo= false;
            }

            if (bool.Parse(Com.Read("PROGRAM:FIM.SINSENTIDO")))
            {
                sinsentido = true;
            }
            else
            {
                sinsentido= false;
            }
        }

        public string GenerarSerial(string Modelo, int Estacion, int Contador)
        {
            string serial = modelo + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                + DateTime.Now.ToString("HH") + Estacion.ToString() + Contador.ToString("D3");
            return serial;
        }

        public void ResetPrograma()
        {
            Thread.Sleep(10);
            //C1.CambioPrograma(0);
            Com.Write("E1_TERMINADO", 1);
            Com.Write("E1_3PASS", 0);
            Com.Write("E1_TAPON_COLOCADO", 0);
            Com.Write("FAIL", 0);
            Thread.Sleep(350);
            Com.Write("E1_TERMINADO", 0);
        }

        //public void Terminar()
        //{
        //    ResetPrograma();
        //}

        ////Hilos?
        //public async void TaskE1()
        //{
        //    MessageBox.Show("hgu");
        //    Serial1 = "1";
        //    //LblME1.Text = "Iniciando Prueba";
        //    //Orifice
        //    TaskO11();

        //    //LblME1.Text = "Realizando Prueba";
        //    //Largo Corrugado

        //    PilotBracketHandler(false);

        //    //ResultadosC1[0] = await C1.PruebaAsync(ResultadosC1[0]);

        //    if (true/*ResultadosC1[0].OKNG*/)
        //    {
        //        LargoCorrugadoBI.SelectColor2 = true;
        //        //LargoCorrugadoBI.Text = ResultadosC1[0].Calificacion.ToString();
        //    }
        //    else
        //    {
        //        LargoCorrugadoBI.SelectColor3 = true;
        //        //LargoCorrugadoBI.Text = //ResultadosC1[0].Calificacion.ToString();
        //        E1Fail = true;

        //    }
        //    //Thread.Sleep(20);
        //    //await C1.CambioProgramaAsync(1);

        //    //Sentido de Corrugado
        //    //ResultadosC1[1] = await C1.PruebaAsync(ResultadosC1[1]);
        //    if (ResultadosC1[1].OKNG)
        //    {
        //        if (ResultadosC1[1].Res == "00")
        //        {
        //            SentidoCorrugadoBI.Text = "D";
        //        }
        //        else if (ResultadosC1[1].Res == "01")
        //        {
        //            SentidoCorrugadoBI.Text = "I";
        //        }
        //        //MessageBox.Show(ResultadosC1[1].Res);
        //        if (sinsentido)
        //        {
        //            SentidoCorrugadoBI.SelectColor2 = true;
        //        }
        //        else if (!sinsentido && ResultadosC1[1].Res == "00")
        //        {
        //            SentidoCorrugadoBI.SelectColor2 = true;
        //        }
        //        else if (!sinsentido && ResultadosC1[1].Res == "01")
        //        {
        //            SentidoCorrugadoBI.SelectColor3 = true;
        //            E1Fail = true;
        //        }
        //        else
        //        {
        //            SentidoCorrugadoBI.SelectColor3 = true;
        //            E1Fail = true;
        //        }
        //    }
        //    else
        //    {
        //        SentidoCorrugadoBI.SelectColor3 = true;
        //        SentidoCorrugadoBI.Text = ResultadosC1[1].Res;
        //        E1Fail = true;
        //    }
        //    //Thread.Sleep(20);
        //    await C1.CambioProgramaAsync(2);

        //    //Nut
        //    ResultadosC1[2] = await C1.PruebaAsync(ResultadosC1[2]);
        //    if (ResultadosC1[2].OKNG)
        //    {
        //        if (ResultadosC1[2].Res == "00")
        //        {
        //            NutBI.Text = "A";
        //        }
        //        else if (ResultadosC1[2].Res == "01")
        //        {
        //            NutBI.Text = "R";
        //        }

        //        if (!nutrojo && ResultadosC1[2].Res == "00")
        //        {
        //            NutBI.SelectColor2 = true;

        //        }
        //        else if (nutrojo && ResultadosC1[2].Res == "01")
        //        {
        //            NutBI.SelectColor2 = true;
        //        }
        //        else
        //        {
        //            NutBI.SelectColor3 = true;
        //            E1Fail = true;
        //        }
        //    }
        //    else
        //    {
        //        NutBI.SelectColor3 = true;
        //        NutBI.Text = ResultadosC1[2].Res;
        //        E1Fail = true;
        //    }
        //    //Thread.Sleep(20);
        //    await C1.CambioProgramaAsync(3);

        //    if (true)
        //    {

        //    }
        //    PilotBracketHandler(false);

        //    if (E1Fail)
        //    {
        //        //LblME1.Text = "Pieza NOK";
        //        E1T = true;
        //        Com.Write("E1_TERMINADO", 1);
        //        Proceso.Abort();
        //    }
        //    else
        //    {
        //        Com.Write("E1_3PASS", 1);
        //        //LblME1.Text = "Esperando Tapón";
        //        ME1.WaitOne();
        //    }




        //    PilotBracketHandler(true);
        //    //Tapon
        //    ResultadosC1[3] = await C1.PruebaAsync(ResultadosC1[3]);
        //    if (ResultadosC1[3].OKNG)
        //    {
        //        TaponBI.SelectColor2 = true;
        //        TaponBI.Text = ResultadosC1[3].Calificacion.ToString();
        //        Com.Write("E1_TAPON_COLOCADO", 1);
        //        Serial1 = GenerarSerial(modelo, 3, E1Contador);
        //        //label15.Text = Serial1.Remove(0, modelo.Length);
        //        //LblME1.Text = "Imprimiendo Etiqueta";
        //        etiquetadora.GenerarEtiqueta(Serial1);
        //        E1Contador++;
        //        Contador.Default.ContadorE1 = E1Contador;
        //        Contador.Default.Save();
        //    }
        //    else
        //    {
        //        TaponBI.SelectColor3 = true;
        //        TaponBI.Text = ResultadosC1[3].Calificacion.ToString();
        //        E1Fail = true;

        //    }
        //    //Thread.Sleep(20);
        //    await C1.CambioProgramaAsync(4);


        //    if (E1Fail)
        //    {
        //        //LblME1.Text = "Pieza NOK";
        //        E1T = true;
        //        Com.Write("E1_TERMINADO", 1);
        //        Proceso.Abort();
        //    }
        //    else
        //    {
        //        //MessageBox.Show(E1T.ToString());
        //        //LblME1.Text = "Esperando Colocación de etiqueta";
        //        ME11.WaitOne();
        //    }


        //    ResultadosC1[4] = await C1.PruebaAsync(ResultadosC1[4]);
        //    if (ResultadosC1[4].OKNG)
        //    {
        //        EtiquetaBI.SelectColor2 = true;
        //        EtiquetaBI.Text = ResultadosC1[4].Calificacion.ToString();
        //        E1Pass = true;
        //        E1T = true;
        //        //LblME1.Text = "Pieza OK";
        //        Com.Write("E1_TERMINADO", 1);
        //    }
        //    else
        //    {
        //        EtiquetaBI.SelectColor3 = true;
        //        EtiquetaBI.Text = ResultadosC1[4].Calificacion.ToString();
        //        E1Fail = true;

        //    }
        //    //Thread.Sleep (20);
        //    await C1.CambioProgramaAsync(0);
        //    if (E1Fail)
        //    {
        //        //LblME1.Text = "Pieza NOK";
        //        E1T = true;
        //        Com.Write("E1_TERMINADO", 1);
        //        Proceso.Abort();
        //    }
        //    else
        //    {
        //        E1T = true;
        //        Proceso.Abort();
        //    }

        //}

        //public async void TaskO11()
        //{
        //    Estructuras.Orifice orifice = await O1.PruebaOrifice();

        //    OrificeBI.Text = orifice.Calificacion.ToString();
        //    if (orifice.OKNG)
        //    {
        //        OrificeBI.SelectColor2 = true;
        //    }
        //    else
        //    {
        //        OrificeBI.SelectColor3 = true;
        //        E1Fail = true;
        //    }
        //}

        //private void PilotBracketHandler(bool determinar)
        //{
        //    if (PilotBracketBI.SelectColor2 == true && !determinar)
        //    {
        //        PilotBracketBI.Color2 = System.Drawing.Color.GreenYellow;
        //    }
        //    else if (PilotBracketBI.SelectColor2 == false && !determinar)
        //    {
        //        PilotBracketBI.SelectColor3 = true;
        //        E1Fail = true;
        //    }
        //    else if (PilotBracketBI.SelectColor2 == true && determinar)
        //    {
        //        PilotBracketBI.Color2 = System.Drawing.Color.Green;
        //        PilotBracketBI.Text = "OK";
        //    }
        //    else
        //    {
        //        PilotBracketBI.SelectColor3 = true;
        //        E1Fail = true;
        //    }
        //}

        //public void IniciarCiclo()
        //{
        //    Proceso = new Thread(TaskE1);
        //    Proceso.Start();
        //}

        //public void InspeccionarTapon()
        //{
        //    ME1.Set();
        //}

        //public void InspeccionarEtiqueta()
        //{
        //    ME11.Set();
        //}

        //public void DetenerCiclo()
        //{
        //    Proceso.Abort();
        //}
    }
}
