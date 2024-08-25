using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace Final_Inspection_Machine_v3._0
{
    public class Etiquetadora2
    {
        bool ConnectionType = true;
        object ZebraLock;

        public Etiquetadora2(bool op)
        {
            ConnectionType = op;
        }

        public string Status()
        {
            string status = " ";
            Connection printerConnection = null;
            try
            {
                printerConnection = GetConnection();
                printerConnection.Open();
                PrinterStatus printerStatus = ZebraPrinterFactory.GetInstance(printerConnection).GetCurrentStatus(); 
                if (printerStatus.isReadyToPrint)
                {
                    status = ("Ready To Print");
                }
                else if (printerStatus.isPaused)
                {
                    status = ("Cannot Print because the printer is paused.");
                }
                else if (printerStatus.isHeadOpen)
                {
                    status = ("Cannot Print because the printer head is open.");
                }
                else if (printerStatus.isPaperOut)
                {
                    status = ("Cannot Print because the paper is out.");
                }
                else
                {
                    status = ("Cannot Print.");
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
                printerConnection.Close();
            }
            return status;
        }

        public Connection GetConnection()
        {
            if (ConnectionType)
            {
                try
                {
                    int port = 9100;
                    return new TcpConnection("192.168.1.30", port);
                }
                catch (Exception e)
                {
                    throw new ConnectionException(e.Message, e);
                }
            }
            else
            {
                try
                {
                    return new UsbConnection("USB001:");
                }
                catch (Exception e)
                {
                    throw new ConnectionException(e.Message, e);
                }
            }
        }

        public void GenerarEtiqueta(string Serial)
        {
            lock(ZebraLock)
            {
                Connection printerConnection = null;
                try
                {
                    printerConnection = GetConnection();
                    printerConnection.Open();

                    Dictionary<int, string> formatVars = new Dictionary<int, string> { { 1, Serial } };
                    ZebraPrinterFactory.GetInstance(printerConnection).PrintStoredFormat("E:ETIQUETA.ZPL", formatVars);
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show(e.Message, "Comunicaction Error");
                }
                catch (ConnectionException e)
                {
                    MessageBox.Show(e.Message, "Comunicaction Error");
                }
                catch (ZebraPrinterLanguageUnknownException e)
                {
                    MessageBox.Show(e.Message, "Comunicaction Error");
                }
                finally
                {
                    if (printerConnection != null)
                    {
                        try
                        {
                            printerConnection.Close();
                        }
                        catch (ConnectionException) { }
                    }
                }
            }
            
        }
    }
}
