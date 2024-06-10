using AdvancedHMIDrivers;
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
    /// Interaction logic for DobleEstacion.xaml
    /// </summary>
    public partial class DobleEstacion : Page
    {
        DataSubscriber Ciclo_En_Curso;
        DataSubscriber Insp_Tapon;
        DataSubscriber Insp_Etiqueta;
        DataSubscriber E_Stop;
        Estacion1 E1;
        Estacion2 E2;
        BarraBaja BarraBaja;
        public string modelo;
        public bool sinsentido, nutrojo, pilotbracket;
        EthernetIPforCLXCom ComCL;

        public DobleEstacion()
        {
            InitializeComponent();
            ComCL = new EthernetIPforCLXCom();
            ComCL.IPAddress = "192.168.1.1";
            ComCL.Timeout = 5000;  
            ComCL.DisableSubscriptions = false;
            ComCL.Port = 44818;
            Ciclo_En_Curso = new DataSubscriber();
            Insp_Tapon = new DataSubscriber();
            Insp_Etiqueta = new DataSubscriber();
            E_Stop = new DataSubscriber();
            Estacion1.NavigationService.Navigate(E1 = new Estacion1(ComCL));
            Estacion2.NavigationService.Navigate(E2 = new Estacion2());
            BarraBajaFrame.NavigationService.Navigate(BarraBaja = new BarraBaja(ComCL));
            Suscripcion();

        }

        private void Suscripcion()
        {
            Ciclo_En_Curso.ComComponent = ComCL;
            Ciclo_En_Curso.PLCAddressValue = new MfgControl.AdvancedHMI.Drivers.PLCAddressItem("CICLO_EN_CURSO");
            Ciclo_En_Curso.DataChanged += Ciclo_En_Curso_DataChanged;

            Insp_Tapon.ComComponent = ComCL;
            Insp_Tapon.PLCAddressValue = new MfgControl.AdvancedHMI.Drivers.PLCAddressItem("INSP_TAPON");
            Insp_Tapon.DataChanged += Insp_Tapon_DataChanged;

            Insp_Etiqueta.ComComponent = ComCL;
            Insp_Etiqueta.PLCAddressValue = new MfgControl.AdvancedHMI.Drivers.PLCAddressItem("INSP_ETIQUETA");
            Insp_Etiqueta.DataChanged += Insp_Etiqueta_DataChanged;

            E_Stop.ComComponent = ComCL;
            E_Stop.PLCAddressValue = new MfgControl.AdvancedHMI.Drivers.PLCAddressItem("Local:1:I.Data.0");
            E_Stop.DataChanged += E_Stop_DataChanged;

            
        }

        private void E_Stop_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            E1.DetenerCiclo();
        }

        private void Insp_Etiqueta_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            if(e.Values[0] == "True")
            {
                if (ComCL.Read("E1_INSP_ETIQUETA") == "True")
                {
                    E1.InspeccionarEtiqueta();
                }

                if (ComCL.Read("E2_INSP_ETIQUETA") == "True")
                {

                }
            }
        }

        private void Insp_Tapon_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            if (e.Values[0] == "True")
            {
                if (ComCL.Read("E1_INSP_TAPON") == "True")
                {
                    E1.InspeccionarTapon();
                }

                if (ComCL.Read("E2_INSP_TAPON") == "True")
                {

                }
            }
        }

        private void Ciclo_En_Curso_DataChanged(object sender, MfgControl.AdvancedHMI.Drivers.Common.PlcComEventArgs e)
        {
            MessageBox.Show(e.Values[1].ToString());
            if (bool.Parse(e.Values[1].ToString()));
            {
                modelo = ComCL.Read("MODELO_SELECCIONADO");
                sinsentido = bool.Parse(ComCL.Read("SINSENTIDO"));
                nutrojo = bool.Parse(ComCL.Read("NUT_ROJO"));
                pilotbracket = bool.Parse(ComCL.Read("PILOT_BRACKET"));
                MessageBox.Show("hgu");
            }
        }

        public void Ajustar()
        {
            //E1.Ajustar();
            //E2.Ajustar();
        }

        public void ActualizarModelo()
        {
            //E1.ActualizarModelo();
        }



    }
}
