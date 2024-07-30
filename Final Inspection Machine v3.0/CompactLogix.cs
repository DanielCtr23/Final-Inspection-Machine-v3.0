using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AdvancedHMIControls;
using AdvancedHMIDrivers;
using MfgControl.AdvancedHMI.Drivers;
using MfgControl.AdvancedHMI.Drivers.Common;

namespace Final_Inspection_Machine_v3._0
{

    public class CompactLogix
    {
        public EthernetIPforCLXCom Com;
        DataSubscriber CicloEnCurso, InspTapon, InspEtiqueta, Estop, Modelo, Mensaje, Seleccionado;

        public event EventHandler IniciarCiclo;
        public event EventHandler InspeccionarTapon;
        public event EventHandler InspeccionarEtiqueta;
        public event EventHandler DetenerCiclo;
        public event EventHandler<bool> CambioModelo;
        public event EventHandler<int> MensajeRecibido;
        public event EventHandler<string> CambioSeleccionado;

        public CompactLogix()
        {
            Com = new EthernetIPforCLXCom();
            Com.IPAddress = "192.168.1.1";
            Com.Timeout = 1000;
            Com.PollRateOverride = 500;
            Inicializar();
        }

        public bool Conexion()
        {
            try
            {
                Com.Read("MODELO_ACEPTADO");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Inicializar()
        {
            
            CicloEnCurso = new DataSubscriber();
            InspTapon = new DataSubscriber();
            InspEtiqueta = new DataSubscriber();
            Estop = new DataSubscriber();
            Modelo = new DataSubscriber();
            Mensaje = new DataSubscriber();
            Seleccionado = new DataSubscriber();

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

            Modelo.ComComponent = Com;
            Modelo.PLCAddressValue = new PLCAddressItem("MODELO_ACEPTADO");
            Modelo.DataChanged += Modelo_DataChanged;

            Mensaje.ComComponent = Com;
            Mensaje.PLCAddressValue = new PLCAddressItem("MENSAJE");
            Mensaje.DataChanged += Mensaje_DataChanged;

            Seleccionado.ComComponent = Com;
            Seleccionado.PLCAddressValue = new PLCAddressItem("MODELO_SELECCIONADO");
            Seleccionado.DataChanged += Seleccionado_DataChanged;

        }

        private void Seleccionado_DataChanged(object sender, PlcComEventArgs e)
        {
            CambioSeleccionado?.Invoke(this, e.Values[0]);
        }

        //Eventos
        private void Mensaje_DataChanged(object sender, PlcComEventArgs e)
        {
            MensajeRecibido?.Invoke(this, int.Parse(e.Values[0]));
        }
        private void Modelo_DataChanged(object sender, PlcComEventArgs e)
        {
            CambioModelo?.Invoke(this, bool.Parse(e.Values[0]));
        }

        private void Estop_DataChanged(object sender, PlcComEventArgs e)
        {
            if (!bool.Parse(e.Values[0]))
            {
                DetenerCiclo?.Invoke(this, e);
            }
        }

        private void InspEtiqueta_DataChanged(object sender, PlcComEventArgs e)
        {
            if (bool.Parse(e.Values[0]))
            {
                InspeccionarEtiqueta?.Invoke(this, e);
            }
        }

        private void InspTapon_DataChanged(object sender, PlcComEventArgs e)
        {
            if (bool.Parse(e.Values[0]))
            {
                InspeccionarTapon?.Invoke(this, e);
            }
        }

        private void CicloEnCurso_DataChanged(object sender, PlcComEventArgs e)
        {
            if (bool.Parse(e.Values[0]))
            {
                IniciarCiclo?.Invoke(this, e);
            }
        }

        //Lecturas en ciclo
        public bool PilotBracket1()
        {
            return bool.Parse(Com.Read("PB_E1_OK"));
        }
        public bool PilotBracket2()
        {
            return bool.Parse(Com.Read("PB_E2_OK"));
        }

        //Lecturas 
        public string ModeloSeleccionado()
        {
            return Com.Read("MODELO_SELECCIONADO");
        }
        public bool Resorte()
        {
            return bool.Parse(Com.Read("RESORTE"));
        }
        public bool PilotBracket()
        {
            return bool.Parse(Com.Read("PILOT_BRACKET"));
        }
        public bool NutRojo()
        {
            try
            {
                return bool.Parse(Com.Read("NUT_ROJO"));
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool SinSentido()
        {
            return bool.Parse(Com.Read("SINSENTIDO"));
        }

        //Escritura
        public void E1_3Pass(bool y)
        {
            Com.Write("E1_3PASS", Convert.ToInt32(y));
        }
        public void E2_3Pass(bool y)
        {
            Com.Write("E2_3PASS", Convert.ToInt32(y));
        }
        public void E1_TAPON_COLOCADO(bool y)
        {
            Com.Write("E1_TAPON_COLOCADO", Convert.ToInt32(y));
        }
        public void E2_TAPON_COLOCADO(bool y)
        {
            Com.Write("E2_TAPON_COLOCADO", Convert.ToInt32(y));
        }

        //Metodos

        public void Terminar()
        {
            try
            {
                Com.Write("E2_TERMINADO", 1);
                Com.Write("E1_TERMINADO", 1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void Cerrar()
        {
            Com.CloseConnection();
        }

    }
}
