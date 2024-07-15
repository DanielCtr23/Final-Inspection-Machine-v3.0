using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdvancedHMIControls;
using AdvancedHMIDrivers;
using MfgControl.AdvancedHMI.Drivers;
using MfgControl.AdvancedHMI.Drivers.Common;


namespace Final_Inspection_Machine_v3._0
{

    public class CompactLogix
    {
        public object Com { get; private set; }
        dynamic comDynamic;
        DataSubscriber CicloEnCurso, InspTapon, InspEtiqueta, Estop, Modelo, Mensaje;

        public event EventHandler IniciarCiclo;
        public event EventHandler InspeccionarTapon;
        public event EventHandler InspeccionarEtiqueta;
        public event EventHandler DetenerCiclo;
        public event EventHandler<bool> CambioModelo;
        public event EventHandler<int> MensajeRecibido;

        public CompactLogix(int op)
        {
            if (op == 1)
            {
                Com = new EthernetIPforCLXCom();
            }
            else if (op == 2)
            {
                Com = new EthernetIPforMicro800();
            }

            if (Com != null)
            {
                comDynamic = Com; // Uso de dynamic para acceder a las propiedades específicas
                comDynamic.IPAddress = "192.168.1.1";
                comDynamic.Timeout = 1000;
                comDynamic.PollRateOverride = 500;

                CicloEnCurso = new DataSubscriber();
                InspTapon = new DataSubscriber();
                InspEtiqueta = new DataSubscriber();
                Estop = new DataSubscriber();
                Modelo = new DataSubscriber();
                Mensaje = new DataSubscriber();

                CicloEnCurso.ComComponent = comDynamic;
                CicloEnCurso.PLCAddressValue = new PLCAddressItem("CICLO_EN_CURSO");
                CicloEnCurso.DataChanged += CicloEnCurso_DataChanged;

                InspTapon.ComComponent = comDynamic;
                InspTapon.PLCAddressValue = new PLCAddressItem("INSP_TAPON");
                InspTapon.DataChanged += InspTapon_DataChanged;

                InspEtiqueta.ComComponent = comDynamic;
                InspEtiqueta.PLCAddressValue = new PLCAddressItem("INSP_ETIQUETA");
                InspEtiqueta.DataChanged += InspEtiqueta_DataChanged;

                Estop.ComComponent = comDynamic;
                Estop.PLCAddressValue = new PLCAddressItem("E_STOP");
                Estop.DataChanged += Estop_DataChanged;

                Modelo.ComComponent = comDynamic;
                Modelo.PLCAddressValue = new PLCAddressItem("MODELO_ACEPTADO");
                Modelo.DataChanged += Modelo_DataChanged;

                Mensaje.ComComponent = comDynamic;
                Mensaje.PLCAddressValue = new PLCAddressItem("MENSAJE");
                Mensaje.DataChanged += Mensaje_DataChanged;
            }
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
            return bool.Parse(comDynamic.Read("PB_E1_OK"));
            
        }
        public bool PilotBracket2()
        {
            return bool.Parse(comDynamic.Read("PB_E2_OK"));
        }

        //Lecturas 
        public string ModeloSeleccionado()
        {
            return comDynamic.Read("MODELO_SELECCIONADO");
        }
        public bool Resorte()
        {
            return bool.Parse(comDynamic.Read("RESORTE"));
        }
        public bool PilotBracket()
        {
            return bool.Parse(comDynamic.Read("PILOT_BRACKET"));
        }
        public bool NutRojo()
        {
            return bool.Parse(comDynamic.Read("NUT_ROJO"));
        }
        public bool SinSentido()
        {
            return bool.Parse(comDynamic.Read("SINSENTIDO"));
        }

        //Escritura
        public void E1_3Pass(bool y)
        {
            comDynamic.Write("E1_3PASS", Convert.ToInt32(y));
        }
        public void E2_3Pass(bool y)
        {
            comDynamic.Write("E2_3PASS", Convert.ToInt32(y));
        }
        public void E1_TAPON_COLOCADO(bool y)
        {
            comDynamic.Write("E1_TAPON_COLOCADO", Convert.ToInt32(y));
        }
        public void E2_TAPON_COLOCADO(bool y)
        {
            comDynamic.Write("E2_TAPON_COLOCADO", Convert.ToInt32(y));
        }

        //Metodos

        public void Terminar()
        {
            comDynamic.Write("E2_TERMINADO", 1);
            comDynamic.Write("E1_TERMINADO", 1);
        }

        public void Cerrar()
        {
            comDynamic.CloseConnection();
        }

    }
}
