using AdvancedHMIControls;
using AdvancedHMIDrivers;
using Final_Inspection_Machine_v3._0.Pages;
using MfgControl.AdvancedHMI.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0
{
    /// <summary>
    /// Lógica de interacción para Seleccion.xaml
    /// </summary>
    public partial class Seleccion : Window
    {
        EthernetIPforCLXCom Com;
        public event EventHandler AceptarModelo;
        public bool[] listo = new bool[2];
        public Seleccion(EthernetIPforCLXCom ClCom)
        {
            InitializeComponent();
            Com = ClCom;
            CargarModelos();
            RigidoFrame.NavigationService.Navigate(new SeleccionRigido(Com,this));
        }

        private void CargarModelos()
        {
            string[] modelos = new string[46];

            for (int i = 0; i < 46; i++)
            {
                modelos[i] = Com.Read("MODELOS[" + i + "]");
            }
            ListaCBx.ItemsSource = modelos;
            ListaCBx.SelectedIndex = ListaCBx.Items.IndexOf(Com.Read("MODELO_SELECCIONADO"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listo[0] && listo[1] && bool.Parse(Com.Read("MODELO_ACEPTADO")))
            {
                AceptarModelo?.Invoke(this, EventArgs.Empty);
                Com.CloseConnection();
                this.Close();
            }
        }

        private void ListaCBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Com.Write("MODELO_SELECCIONADO", ListaCBx.SelectedItem.ToString());
        }
    }
}
