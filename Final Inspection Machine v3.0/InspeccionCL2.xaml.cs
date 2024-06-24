using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Lógica de interacción para InspeccionCL2.xaml
    /// </summary>
    public partial class InspeccionCL2 : Window
    {
        CompactLogix Com;
        public InspeccionCL2()
        {
            InitializeComponent();
            Com = new CompactLogix();
            Com.CambioModelo += Com_CambioModelo;
            Com.IniciarCiclo += Com_IniciarCiclo;
        }

        private void Com_IniciarCiclo(object sender, EventArgs e)
        {
        }

        private void Com_CambioModelo(object sender, bool e)
        {
            ModeloBtn.Content = Com.ModeloSeleccionado();
            if (e)
            {
                ModeloBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38,38,38));
            }
            else
            {
                ModeloBtn.Background = new SolidColorBrush(Colors.Red);
            }
        }

    }
}
