using SciChart.Core.Extensions;
using ScottPlot.Colormaps;
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

            //MessageBox.Show(detalle.Rows[0]["Pass"].ToString());
            //MessageBox.Show(detalle.Rows[0]["Fail"].ToString());

            bool pass = detalle.Rows[0]["Pass"].ToString() == "1";
            bool fail = detalle.Rows[0]["Fail"].ToString() == "1";

            bool E = (detalle.Rows[0]["Fail"].ToString() == ""  || detalle.Rows[0]["Pass"].ToString() == "");

            //bool pass = bool.Parse(int.Parse(detalle.Rows[0]["Pass"].ToString()));
            //bool fail = bool.Parse(detalle.Rows[0]["Fail"].ToString());

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

            if (pass && !fail && !E)
            {
                PassTB.Text = "PASS";
                PassTB.Foreground = Brushes.Green;
            }
            else if (!pass && fail && !E)
            {
                PassTB.Text = "FAIL";
                PassTB.Foreground = Brushes.Red;
            }
            else
            {
                PassTB.Text = "N/A o N/R";
                PassTB.Foreground = Brushes.Yellow;
            }

            if (detalle.Rows[0]["Rosca"].ToString().StartsWith("PASS"))
            {
                RoscaTB.Foreground = Brushes.Green;
            }
            else
            {
                RoscaTB.Foreground = Brushes.Red;
            }
            if (detalle.Rows[0]["Crack"].ToString().StartsWith("PASS"))
            {
                CrackTB.Foreground = Brushes.Green;
            }
            else
            {
                CrackTB.Foreground = Brushes.Red;
            }
            if (detalle.Rows[0]["Resorte"].ToString().StartsWith("PASS"))
            {
                ResorteTB.Foreground = Brushes.Green;
            }
            else
            {
                ResorteTB.Foreground = Brushes.Red;
            }
            if (detalle.Rows[0]["PilotBracket"].ToString().StartsWith("PASS"))
            {
                PilotBracketTB.Foreground = Brushes.Green;
            }
            else
            {
                PilotBracketTB.Foreground = Brushes.Red;
            }
            if (detalle.Rows[0]["Largo"].ToString().StartsWith("PASS"))
            {
                LargoTB.Foreground = Brushes.Green;
            }
            else
            {
                LargoTB.Foreground = Brushes.Red;
            }
            if (detalle.Rows[0]["Sentido"].ToString().StartsWith("PASS"))
            {
                SentidoTB.Foreground = Brushes.Green;
            }
            else
            {
                SentidoTB.Foreground = Brushes.Red;
            }
            if (detalle.Rows[0]["Nut"].ToString().StartsWith("PASS"))
            {
                NutTB.Foreground = Brushes.Green;
            }
            else
            {
                NutTB.Foreground = Brushes.Red;
            }

            if (detalle.Rows[0]["Tapon"].ToString().StartsWith("PASS"))
            {
                TaponTB.Foreground = Brushes.Green;
            }
            else
            {
                TaponTB.Foreground = Brushes.Red;
            }

            if (detalle.Rows[0]["Etiqueta"].ToString().StartsWith("PASS"))
            {
                EtiquetaTB.Foreground = Brushes.Green;
            }
            else
            {
                EtiquetaTB.Foreground = Brushes.Red;
            }


        }
    }
}
