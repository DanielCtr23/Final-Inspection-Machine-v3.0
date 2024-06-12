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

namespace Final_Inspection_Machine_v3._0
{
    public partial class Inspeccion_CompactLogix
    {
        EthernetIPforCLXCom Com;
        DataSubscriber CicloEnCurso, InspTapon, InspEtiqueta, Estop;
        Thread Estacion1, Estacion2;

        static ManualResetEvent EsperarTaponE1, EsperarTaponE2;
        static ManualResetEvent EsperarEtiquetaE1, EsperarEtiquetaE2;

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
            Estop.PLCAddressValue = new PLCAddressItem("E_STOP");
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

            if (bool.Parse(Com.Read("NUT_ROJO")))
            {
                nutrojo = true;
            }
            else
            {
                nutrojo = false;
            }
            
            if (bool.Parse(Com.Read("SINSENTIDO")))
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
                Estacion1 = new Thread(TaskE1);
                Estacion2 = new Thread(TaskE2);
                Estacion1.Start();
                Estacion2.Start();
            }
        }

        private void InicializarCamaras()
        {
            IPAddress IPC1 = IPAddress.Parse("192.168.1.2");
            Corrugado1 = new IV3(IPC1, 8500);
            IPAddress IPC2 = IPAddress.Parse("192.168.1.3");
            Corrugado2 = new IV3(IPC2, 8500);

            IPAddress IPO11 = IPAddress.Parse("192.168.1.4");
            Orifice11 = new IV3(IPO11, 8500);
            IPAddress IPO12 = IPAddress.Parse("192.168.1.5");
            Orifice12 = new IV3(IPO12, 8500);

            IPAddress IPO21 = IPAddress.Parse("192.168.1.6");
            Orifice21 = new IV3(IPO21, 8500);
            IPAddress IPO22 = IPAddress.Parse("192.168.1.7");
            Orifice22 = new IV3(IPO22, 8500);
        }


        private async void TaskE1()
        {


            EsperarTaponE1.WaitOne();

            EsperarEtiquetaE1.WaitOne();


        }

        private async void TaskE2()
        {
            TaskO1();

            EsperarTaponE2.WaitOne();

            EsperarEtiquetaE2.WaitOne();

        }

        private async void TaskO1()
        {

        }

        private async void TaskO2()
        {

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


    }
}
