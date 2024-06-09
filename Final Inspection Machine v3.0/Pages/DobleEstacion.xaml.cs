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
    /// Interaction logic for DobleEstacion.xaml
    /// </summary>
    public partial class DobleEstacion : Page
    {
        
        public DobleEstacion()
        {
            InitializeComponent();
        }

        public void Ajustar()
        {
            var Page1 = Estacion1.Content as Estacion1;
            var Page2 = Estacion2.Content as Estacion2;
            Page1.Ajustar();
            Page2.Ajustar();
        }
    }
}
