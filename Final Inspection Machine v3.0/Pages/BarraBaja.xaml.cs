using AdvancedHMIControls;
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
        Seleccion Seleccion = new Seleccion();
        public BarraBaja()
        {
            InitializeComponent();
            ModelosLabel.Text = "AS48378B";
            ModelosHost.Child = ModelosLabel;
        }

        private void RegresarBtn_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Seleccion.ShowDialog();
            }
            catch (Exception)
            {
                Seleccion.Focus();
            }
        }
    }
}
