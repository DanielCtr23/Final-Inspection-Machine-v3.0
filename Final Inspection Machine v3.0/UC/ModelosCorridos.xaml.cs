using SciChart.Data.Model;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Interaction logic for ModelosCorridos.xaml
    /// </summary>
    public partial class ModelosCorridos : System.Windows.Controls.UserControl
    {
        DataManager DM = new DataManager();
        public ModelosCorridos()
        {

            InitializeComponent();
            Refresh(false);
            
        }

        public void Refresh(bool TE)
        {
            if (TE)
            {
                DG.ItemsSource = DM.ProduccionModelos("TE").DefaultView;
            }
            else
            {
                DG.ItemsSource = DM.ProduccionModelos("TN").DefaultView;
            }
        }

        

        
    }
}
