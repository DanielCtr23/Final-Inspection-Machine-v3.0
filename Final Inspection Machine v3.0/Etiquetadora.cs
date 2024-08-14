using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace Final_Inspection_Machine_v3._0
{
    internal class Etiquetadora
    {
        private Zebra.Sdk.Comm.Connection Impresora;
        private ZebraPrinter genericPrinter;
        private ZebraPrinterLinkOs linkOsPrinter;
        private readonly object locko = new object();
        private bool status = false;

        public Etiquetadora()
        {
            Inicializar();
        }

        public bool Conectada()
        {
            return (Impresora.Connected && linkOsPrinter.Connection.Connected && genericPrinter.Connection.Connected);
        }

        public void Cerrar()
        {
            Impresora.Close();
        }



        private void Inicializar()
        {
            lock (locko)
            {
                try
                {
                    Impresora = new Zebra.Sdk.Comm.TcpConnection("192.168.1.30", 6101);
                    Impresora.Open();
                    genericPrinter = ZebraPrinterFactory.GetInstance(Impresora);
                    linkOsPrinter = ZebraPrinterFactory.CreateLinkOsPrinter(genericPrinter);
                    status = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inicializando la impresora: {ex.Message}");
                    status = false;
                }
            }
        }

        public void GenerarEtiqueta(string Serial)
        {
            lock (locko)
            {
                try
                {
                    if (!Impresora.Connected || !linkOsPrinter.Connection.Connected || !genericPrinter.Connection.Connected)
                    {
                        Inicializar();
                        if (linkOsPrinter != null)
                        {
                            var vars = new Dictionary<int, string> { { 1, Serial } };
                            linkOsPrinter.PrintStoredFormatWithVarGraphics("E:ETIQUETA.ZPL", vars);
                        }
                    }
                    else
                    {
                        if (linkOsPrinter != null)
                        {
                            var vars = new Dictionary<int, string> { { 1, Serial } };
                            linkOsPrinter.PrintStoredFormat("E:ETIQUETA.ZPL", vars);
                        }
                    }

                }
                catch (ConnectionException e)
                {
                    //MessageBox.Show($"Error de conexión: {e.Message}"); 
                    try
                    {
                        if (!Impresora.Connected)
                        {
                            try
                            {
                                Impresora.Close();
                                linkOsPrinter.Connection.Close();
                                genericPrinter.Connection.Close();
                            }
                            catch (Exception)
                            {

                            }
                            Inicializar();
                            if (linkOsPrinter != null)
                            {
                                var vars = new Dictionary<int, string> { { 1, Serial } };
                                linkOsPrinter.PrintStoredFormatWithVarGraphics("E:ETIQUETA.ZPL", vars);
                            }
                        }
                        else
                        {
                            if (linkOsPrinter != null)
                            {
                                var vars = new Dictionary<int, string> { { 1, Serial } };
                                linkOsPrinter.PrintStoredFormat("E:ETIQUETA.ZPL", vars);
                            }
                        }

                    }
                    catch
                    {

                    }
                }
                catch (ZebraPrinterLanguageUnknownException e)
                {
                    MessageBox.Show($"Error de lenguaje de impresora desconocido: {e.Message}");
                }
                catch (Exception e)
                {
                    try
                    {
                        Impresora.Close();
                        Inicializar();
                        if (linkOsPrinter != null)
                        {
                            var vars = new Dictionary<int, string> { { 1, Serial } };
                            linkOsPrinter.PrintStoredFormatWithVarGraphics("E:ETIQUETA.ZPL", vars);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    
                }
            }
        }

    }
}
