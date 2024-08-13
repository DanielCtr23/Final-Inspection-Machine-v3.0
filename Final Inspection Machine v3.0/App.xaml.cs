using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "Final_Inspection_Machine_v3.0";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // Si no se ha creado un nuevo mutex, significa que ya existe una instancia
                MessageBox.Show("La aplicación ya se está ejecutando.");
                Application.Current.Shutdown();
                return;
            }

            Log("La aplicacion se inicio");
            Application.Current.Exit += new ExitEventHandler(OnApplicationExit);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(OnDispatcherUnhandledException);

            RenderOptions.ProcessRenderMode = RenderMode.Default; // Puede ser Default, SoftwareOnly, o HardwareOnly
            base.OnStartup(e);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log("La aplicacion se cerro de manera inesperada / " + e.Exception.ToString());
            e.Handled = true;
            string applicationPath = Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(applicationPath);

            Application.Current.Shutdown();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log("La aplicacion se cerro de manera inesperada / " + e.ExceptionObject.ToString());
            string applicationPath = Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(applicationPath);

            Application.Current.Shutdown();
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Log("La aplicacion se cerro de manera controlada");
        }

        private void Log(String mensaje)
        {
            string logFilePath = "App_Log.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}:{mensaje}");
            }
        }
    }
}
