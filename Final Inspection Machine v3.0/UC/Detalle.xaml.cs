using SciChart.Core.Extensions;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Interaction logic for Detalle.xaml
    /// </summary>
    public partial class Detalle : UserControl
    {
        DataManager DM = new DataManager();
        public Detalle()
        {
            InitializeComponent();
        }

        public void CargarDetalle(int id)
        {
            DataTable detalle = new DataTable();
            detalle = DM.Detalle(id);

            SerialTB.Text = detalle.Rows[0]["Serial"].ToString();
            FechaTB.Text = detalle.Rows[0]["Fecha"].ToString();
            RoscaTB.Text = detalle.Rows[0]["Rosca"].ToString();
            CrackTB.Text = detalle.Rows[0]["Crack"].ToString();
            ResorteTB.Text = detalle.Rows[0]["Resorte"].ToString();
            PilotBracketTB.Text = detalle.Rows[0]["PilotBracket"].ToString();
            LargoTB.Text = detalle.Rows[0]["Largo"].ToString();
            SentidoTB.Text = detalle.Rows[0]["Sentido"].ToString();
            NutTB.Text = detalle.Rows[0]["Nut"].ToString();
            TaponTB.Text = detalle.Rows[0]["Tapon"].ToString();
            EtiquetaTB.Text = detalle.Rows[0]["Etiqueta"].ToString();

        }
    }
}
