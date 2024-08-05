using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        
        object locko = new object();

        ZebraPrinter genericPrinter;
        ZebraPrinterLinkOs linkOsPrinter;
        public Etiquetadora()
        {
            Inicializar();
        }

        public void Inicializar()
        {
            try
            {
                Impresora = new Zebra.Sdk.Comm.TcpConnection("192.168.1.30", 6101);
                Impresora.Open();
                genericPrinter = ZebraPrinterFactory.GetInstance(Impresora);
                linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(genericPrinter);
            }
            catch (Exception)
            {
            }
        }


        public void GenerarEtiqueta(string Serial)
        {
                //Connection cnZebra = new UsbConnection("USB001:");
            try
            {
                if (!Impresora.Connected)
                {
                    try
                    {
                        Impresora.Open();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                //Impresora.Open();

                if (linkOsPrinter != null)
                {
                    Dictionary<int, string> vars = new Dictionary<int, string> {  { 1, Serial }, };
                    //linkOsPrinter.PrintStoredFormatWithVarGraphics("E:ETIQUETA.ZPL", vars);
                    linkOsPrinter.PrintStoredFormat("E:ETIQUETA.ZPL", vars);

                }
            }
            catch (ConnectionException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (ZebraPrinterLanguageUnknownException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                //Impresora.Close();
            }
            

        }
    }
}
