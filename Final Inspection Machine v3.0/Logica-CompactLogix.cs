using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdvancedHMIControls;
using AdvancedHMIDrivers;
using MfgControl.AdvancedHMI.Drivers;
using MfgControl.AdvancedHMI.Drivers.Common;

using IV3_Keyence;
using System.Net;
using System.Windows;

namespace Final_Inspection_Machine_v3._0
{
    public partial class Inspeccion_CompactLogix
    {
        EthernetIPforCLXCom Com;
        DataSubscriber CicloEnCurso, InspTapon, InspEtiqueta, Estop;
        Thread Estacion1, Estacion2, HiloPrincipal;

        static ManualResetEvent EsperarTaponE1, EsperarTaponE2;
        static ManualResetEvent EsperarEtiquetaE1, EsperarEtiquetaE2;

        Etiquetadora etiquetadora;
        DB dB;

        string modelo, serial1, serial2;
        int Contador1, Contador2;
        int malas, buenas;

        private void InicializarPLC()
        {
            Com = new EthernetIPforCLXCom()
            {
                IPAddress = "192.168.1.1",
                Timeout = 2000,
                PollRateOverride = 500
            };

            CicloEnCurso = new DataSubscriber();
            InspTapon = new DataSubscriber();
            InspEtiqueta = new DataSubscriber();
            Estop = new DataSubscriber();

            CicloEnCurso.ComComponent = Com;
            CicloEnCurso.PLCAddressValue = new PLCAddressItem("CICLO_EN_CURSO");
            CicloEnCurso.DataChanged += CicloEnCurso_DataChanged;

            InspTapon.ComComponent = Com;
            InspTapon.PLCAddressValue = new PLCAddressItem("INSP_TAPON");
            InspTapon.DataChanged += InspTapon_DataChanged; 

            InspEtiqueta.ComComponent = Com;
            InspEtiqueta.PLCAddressValue = new PLCAddressItem("INSP_ETIQUETA");
            InspEtiqueta.DataChanged += InspEtiqueta_DataChanged;

            Estop.ComComponent = Com;
            Estop.PLCAddressValue = new PLCAddressItem("INPUT_BITS.0");
            Estop.DataChanged += Estop_DataChanged;

            ModeloBI.ComComponent = Com;
            ModeloBI.PLCAddressText = "MODELO_SELECCIONADO";
            ModeloBI.PLCAddressSelectColor2 = "MODELO_ACEPTADO";
            ModeloBI.Click += ModeloBI_Click;
            ModeloBI.TextChanged += ModeloBI_TextChanged;
            


        }

        private void ModeloBI_TextChanged(object sender, EventArgs e)
        {
            if (bool.Parse(Com.Read("PILOT_BRACKET")))
            {
                OcultarPilotBracket(false);
                pilotbracket = true;
            }
            else
            {
                OcultarPilotBracket(true);
                pilotbracket = false ;
            }

            if (bool.Parse(Com.Read("PROGRAM:FIM.NUT_ROJO")))
            {
                nutrojo = true;
            }
            else
            {
                nutrojo = false;
            }
            
            if (bool.Parse(Com.Read("PROGRAM:FIM.SINSENTIDO")))
            {
                sinsentido = true;
            }
            else
            {
                sinsentido = false;
            }

            if (bool.Parse(Com.Read("RESORTE")))
            {
                OcultarResorte(false);
                resorte = true;
            }
            else
            {
                OcultarResorte(true);
                resorte = false;
            }

        }

        private void ModeloBI_Click(object sender, EventArgs e)
        {
            //Seleccion de Modelo
        }

        private void Estop_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            if (!bool.Parse(e.Values[0]))
            {
                //MessageBox.Show("Stop");
                try
                {
                    HiloPrincipal.Abort();
                }
                catch (Exception)
                {

                }
                Resetprograma();
                LimpiarPantalla();
            }
        }

        private void InspEtiqueta_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            if (bool.Parse(e.Values[0]))
            {
                EsperarEtiquetaE1.Set();
                EsperarEtiquetaE2.Set();
            }
        }

        private void InspTapon_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            if (bool.Parse(e.Values[0]))
            {
                EsperarTaponE1.Set();
                EsperarTaponE2.Set();
            }
        }

        private void CicloEnCurso_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            if (bool.Parse(e.Values[0]))
            {
                EsperarTaponE1 = new ManualResetEvent(true);
                EsperarTaponE2 = new ManualResetEvent(true);
                EsperarEtiquetaE1 = new ManualResetEvent(true);
                EsperarEtiquetaE2 = new ManualResetEvent(true);

                //HiloPrincipal = new Thread(Ejecucion);
                //HiloPrincipal.Start();
            }
        }

        private void InicializarCamaras()
        {
            IPAddress IPC1 = IPAddress.Parse("192.168.1.2");
            Corrugado1 = new IV3(IPC1, 8500);
            IPAddress IPC2 = IPAddress.Parse("192.168.1.3");
            Corrugado2 = new IV3(IPC2, 8500);

            //IPAddress IPO11 = IPAddress.Parse("192.168.1.4");
            //Orifice11 = new IV3(IPO11, 8500);
            //IPAddress IPO12 = IPAddress.Parse("192.168.1.5");
            //Orifice12 = new IV3(IPO12, 8500);

            //IPAddress IPO21 = IPAddress.Parse("192.168.1.6");
            //Orifice21 = new IV3(IPO21, 8500);
            //IPAddress IPO22 = IPAddress.Parse("192.168.1.7");
            //Orifice22 = new IV3(IPO22, 8500);
        }

        private void Ejecucion()
        {
            serial1 = 1.ToString();
            serial2 = 2.ToString();
            Estacion1 = new Thread(TaskE1);
            Estacion2 = new Thread(TaskE2);
            Estacion1.Start();
            Estacion2.Start();
            Estacion1.Join();
            Estacion2.Join();


            if (Fail[0])
            {
                malas++;
                dB.Guardar(serial1, modelo, DateTime.Now, false);
            }
            if (Fail[1])
            {
                malas++;
                dB.Guardar(serial2, modelo, DateTime.Now, false);
            }

            Fail[0] = false;
            Fail[1] = false;

            if (Pass[0])
            {
                buenas++;
                dB.Guardar(serial1, modelo, DateTime.Now, true);
            }
            if (Pass[1])
            {
                buenas++;
                dB.Guardar(serial2, modelo, DateTime.Now, true);
            }

            Pass[0] = false;
            Pass[1] = false;

            LimpiarPantalla();
            Resetprograma();

        }

        

        

        

        private void LimpiarPantalla()
        {
            OrificeBI1.SelectColor2 = false;
            OrificeBI1.SelectColor3 = false;
            PilotBracketBI1.SelectColor2 = false;
            PilotBracketBI1.SelectColor3 = false;
            ResorteBI1.SelectColor2 = false;
            ResorteBI1.SelectColor3 = false;
            LargoBI1.SelectColor2 = false;
            LargoBI1.SelectColor3 = false;
            SentidoBI1.SelectColor2 = false;
            SentidoBI1.SelectColor3 = false;
            NutBI1.SelectColor2 = false;
            NutBI1.SelectColor3 = false;
            EtiquetaBI1.SelectColor2 = false;
            EtiquetaBI1.SelectColor3 = false;

            OrificeBI2.SelectColor2 = false;
            OrificeBI2.SelectColor3 = false;
            PilotBracketBI2.SelectColor2 = false;
            PilotBracketBI2.SelectColor3 = false;
            ResorteBI2.SelectColor2 = false;
            ResorteBI2.SelectColor3 = false;
            LargoBI2.SelectColor2 = false;
            LargoBI2.SelectColor3 = false;
            SentidoBI2.SelectColor2 = false;
            SentidoBI2.SelectColor3 = false;
            NutBI2.SelectColor2 = false;
            NutBI2.SelectColor3 = false;
            EtiquetaBI2.SelectColor2 = false;
            EtiquetaBI2.SelectColor3 = false;
            EtiquetaBI2.SelectColor2 = false;
            EtiquetaBI2.SelectColor3 = false;
        }

        private void Terminar()
        {

        }

        private void InicializarPruebas()
        {

        }

        public string GenerarSerial(string Modelo, int Estacion, int Contador)
        {
            string serial = modelo + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                + DateTime.Now.ToString("HH") + Estacion.ToString() + Contador.ToString("D3");
            return serial;
        }

        private void Resetprograma()
        {
            Corrugado1.CambioPrograma(0);
            Corrugado1.CambioPrograma(0);
            Com.Write("E1_TERMINADO0", 1);
            Com.Write("E2_TERMINADO0", 1);
            Com.Write("E1_3PASS", 0);
            Com.Write("E2_3PASS", 0);
            Com.Write("E1_TAPON_COLOCADO", 0);
            Com.Write("E2_TAPON_COLOCADO", 0);
            Com.Write("FAIL", 0);
            Com.Write("PASS", 0);

            Thread.Sleep(380);

            Com.Write("E1_TERMINADO0", 0);
            Com.Write("E2_TERMINADO0", 0);
        }
    }
}
