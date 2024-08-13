using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

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

            RenderOptions.ProcessRenderMode = RenderMode.Default; // Puede ser Default, SoftwareOnly, o HardwareOnly
            base.OnStartup(e);
        }
    }
}
