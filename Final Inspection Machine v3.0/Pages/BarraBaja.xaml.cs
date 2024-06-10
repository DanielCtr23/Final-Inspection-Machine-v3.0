using AdvancedHMIControls;
using AdvancedHMIDrivers;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.Pages
{
    /// <summary>
    /// Interaction logic for BarraBaja.xaml
    /// </summary>
    public partial class BarraBaja : Page
    {
        BasicLabel ModelosLabel = new BasicLabel();
        Seleccion Seleccion;
        EthernetIPforCLXCom Com;
        public BarraBaja(EthernetIPforCLXCom ClCom)
        {
            InitializeComponent();
            Com = ClCom;
            //ModelosLabel.Text = "AS48378B";
            //ModelosHost.Child = ModelosLabel;
        }

        private void RegresarBtn_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BasicIndicator_Click(object sender, EventArgs e)
        {
            try
            {
                Seleccion = new Seleccion(Com);
                Seleccion.AceptarModelo += (senders, args) =>
                {
                    DobleEstacion DobleEstacion = this.Parent as DobleEstacion;
                    DobleEstacion.ActualizarModelo();
                };
                Seleccion.ShowDialog();
            }
            catch (Exception)
            {
                Seleccion.Focus();
            }
        }

        private void ModelosHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
