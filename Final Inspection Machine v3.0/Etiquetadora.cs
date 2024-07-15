using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Final_Inspection_Machine_v3._0
{
    internal class Etiquetadora
    {
        Zebra.Sdk.Comm.Connection Impresora;
        public Thread Conexion;
        bool status = false;
        public Etiquetadora()
        {
            Inicializar();
        }

        public void Inicializar()
        {
            try
            {
                Impresora = new Zebra.Sdk.Comm.TcpConnection("192.168.1.30", 6101);
            }
            catch (Exception)
            {
            }
        }

        public bool ImprimirEtiqueta(String ZPL)
        {

            try
            {
                Impresora.Open();
                Impresora.Write(Encoding.UTF8.GetBytes(ZPL));
                Impresora.Close();

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
                throw;
            }
        }

        private readonly object printerLock = new object();

        public void GenerarEtiqueta(string Serial)
        {
            //Connection cnZebra = new UsbConnection("USB001:");
            try
            {
                Impresora.Open();
                ZebraPrinter genericPrinter = ZebraPrinterFactory.GetInstance(Impresora);
                ZebraPrinterLinkOs linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(genericPrinter);

                if (linkOsPrinter != null)
                {
                    Dictionary<int, string> vars = new Dictionary<int, string> {
                    { 1, Serial },
                };

                    linkOsPrinter.PrintStoredFormatWithVarGraphics("E:ETIQUETA.ZPL", vars);
                }
            }
            catch (ConnectionException e)
            {
            }
            catch (ZebraPrinterLanguageUnknownException e)
            {
            }
            finally
            {
                Impresora.Close();
            }

        }
    }
}
