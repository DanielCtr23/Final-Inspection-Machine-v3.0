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
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio : Page
    {
        public Inicio()
        {
            InitializeComponent();
            EficienciaActualChart.BackgroundArcColor = Colors.Red;
            ProduccionActualChart.UpdateDefaultStyle();
        }

        private void Chart270UC_Loaded(object sender, RoutedEventArgs e)
        {
            ProduccionActualChart.Value = 90;
            EficienciaActualChart.Value = (187*100)/192;
            EficienciaActualChart.Text1 = ((18700/192)).ToString()+"%";
            EficienciaActualChart.Text2 = "187/192/5";

            var items = new List<object>
            {
                new { Modelo = "AS48378B", Cantidad = 25 },
                new { Modelo = "AS48378D", Cantidad = 30 },
            };

            ListaModelosCorridos.ItemsSource = items;

        }
    }
}
