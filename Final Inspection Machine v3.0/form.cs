using AdvancedHMIDrivers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Inspection_Machine_v3._0
{
    public partial class form : Form
    {
        bool permiso;
        bool listo = false;
        EthernetIPforCLXCom Com;
        public form(EthernetIPforCLXCom Com)
        {
            InitializeComponent();
            this.Com = Com;
            string[] modelos = new string[46];
            for (int i = 0; i < 46; i++)
            {
                modelos[i] = Com.Read("MODELOS[" + i.ToString() + "]");
            }
            comboBox1.DataSource = modelos.ToList();
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(Com.Read("MODELO_SELECCIONADO"));
        }

        private void form_Load(object sender, EventArgs e)
        {
            BI_CORRUGADOB_V.ComComponent = Com;
            BI_CORRUGADOC_V.ComComponent = Com;
            BI_CORRUGADOD_V.ComComponent = Com;
            BI_CORRUGADOE_V.ComComponent = Com;
            BI_CORRUGADOF_V.ComComponent = Com;

            BI_RIGIDOB_V.ComComponent = Com;
            BI_RIGIDOC_V.ComComponent = Com;
            BI_RIGIDOD_V.ComComponent = Com;
            BI_RIGIDOE_V.ComComponent = Com;
            BI_RIGIDOF_V.ComComponent = Com;
            BI_RIGIDOG_V.ComComponent = Com;

            BI_CORRUGADOB_V.PLCAddressSelectColor3 = "SENSOR_CB";
            BI_CORRUGADOC_V.PLCAddressSelectColor3 = "SENSOR_CC";
            BI_CORRUGADOD_V.PLCAddressSelectColor3 = "SENSOR_CD";
            BI_CORRUGADOE_V.PLCAddressSelectColor3 = "SENSOR_CE";
            BI_CORRUGADOF_V.PLCAddressSelectColor3 = "SENSOR_CF";

            BI_RIGIDOB_V.PLCAddressSelectColor3 = "SENSOR_RB";
            BI_RIGIDOC_V.PLCAddressSelectColor3 = "SENSOR_RC";
            BI_RIGIDOD_V.PLCAddressSelectColor3 = "SENSOR_RD";
            BI_RIGIDOE_V.PLCAddressSelectColor3 = "SENSOR_RE";
            BI_RIGIDOF_V.PLCAddressSelectColor3 = "SENSOR_RF";
            BI_RIGIDOG_V.PLCAddressSelectColor3 = "SENSOR_RG";

            BI_CORRUGADOB_V.PLCAddressSelectColor2 = "POSICION_CORRUGADO_B";
            BI_CORRUGADOC_V.PLCAddressSelectColor2 = "POSICION_CORRUGADO_C";
            BI_CORRUGADOD_V.PLCAddressSelectColor2 = "POSICION_CORRUGADO_D";
            BI_CORRUGADOE_V.PLCAddressSelectColor2 = "POSICION_CORRUGADO_E";
            BI_CORRUGADOF_V.PLCAddressSelectColor2 = "POSICION_CORRUGADO_F";

            BI_RIGIDOB_V.PLCAddressSelectColor2 = "POSICION_RIGIDO_B";
            BI_RIGIDOC_V.PLCAddressSelectColor2 = "POSICION_RIGIDO_C";
            BI_RIGIDOD_V.PLCAddressSelectColor2 = "POSICION_RIGIDO_D";
            BI_RIGIDOE_V.PLCAddressSelectColor2 = "POSICION_RIGIDO_E";
            BI_RIGIDOF_V.PLCAddressSelectColor2 = "POSICION_RIGIDO_F";
            BI_RIGIDOG_V.PLCAddressSelectColor2 = "POSICION_RIGIDO_G";
            timer4.Start();

            if (bool.Parse(Com.Read("MODELO_ACEPTADO")))
            {
                label3.Text = "Modelo Aceptado";
                label3.ForeColor = Color.Black;
                label3.BackColor = Color.Green;
                permiso = true;
            }
            else
            {
                label3.Text = "No corresponde el modelo seleccionado con la posición actual";
                label3.ForeColor = Color.Red;
                label3.BackColor = Color.Yellow;
                permiso = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listo)
            {
                Com.Write("MODELO_SELECCIONADO", comboBox1.SelectedItem.ToString());
            }
            if (bool.Parse(Com.Read("MODELO_ACEPTADO")))
            {
                label3.Text = "Modelo Aceptado";
                label3.ForeColor = Color.Black;
                label3.BackColor = Color.Green;
                permiso = true;
            }
            else
            {
                label3.Text = "No corresponde el modelo seleccionado con la posición actual";
                label3.ForeColor = Color.Red;
                label3.BackColor = Color.Yellow;
                permiso = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = "l";
            label3.ForeColor = Color.Black;
            label3.BackColor = Color.Transparent;
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.Text = "Modelo Aceptado";
            label3.ForeColor = Color.Black;
            label3.BackColor = Color.Green;
            timer2.Stop();
            this.Close();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (bool.Parse(Com.Read("MODELO_ACEPTADO")))
            {
                label3.Text = "Modelo Aceptado";
                label3.ForeColor = Color.Black;
                label3.BackColor = Color.Green;
                permiso = true;
            }
            else
            {
                label3.Text = "No corresponde el modelo seleccionado con la posición actual";
                label3.ForeColor = Color.Red;
                label3.BackColor = Color.Yellow;
                permiso = false;
            }

            if (BI_RIGIDOB_V.SelectColor2 && BI_RIGIDOB_V.SelectColor3)
            {
                BI_RIGIDOB_V.Color2 = Color.Green;
            }
            else
            {
                BI_RIGIDOB_V.Color2 = Color.Yellow;
            }

            if (BI_RIGIDOC_V.SelectColor2 && BI_RIGIDOC_V.SelectColor3)
            {
                BI_RIGIDOC_V.Color2 = Color.Green;
            }
            else
            {
                BI_RIGIDOC_V.Color2 = Color.Yellow;
            }

            if (BI_RIGIDOD_V.SelectColor2 && BI_RIGIDOD_V.SelectColor3)
            {
                BI_RIGIDOD_V.Color2 = Color.Green;
            }
            else
            {
                BI_RIGIDOD_V.Color2 = Color.Yellow;
            }

            if (BI_RIGIDOE_V.SelectColor2 && BI_RIGIDOE_V.SelectColor3)
            {
                BI_RIGIDOE_V.Color2 = Color.Green;
            }
            else
            {
                BI_RIGIDOE_V.Color2 = Color.Yellow;
            }

            if (BI_RIGIDOF_V.SelectColor2 && BI_RIGIDOF_V.SelectColor3)
            {
                BI_RIGIDOF_V.Color2 = Color.Green;
            }
            else
            {
                BI_RIGIDOF_V.Color2 = Color.Yellow;
            }

            if (BI_RIGIDOG_V.SelectColor2 && BI_RIGIDOG_V.SelectColor3)
            {
                BI_RIGIDOG_V.Color2 = Color.Green;
            }
            else
            {
                BI_RIGIDOG_V.Color2 = Color.Yellow;
            }

            if (BI_CORRUGADOB_V.SelectColor2 && BI_CORRUGADOB_V.SelectColor3)
            {
                BI_CORRUGADOB_V.Color2 = Color.Green;
            }
            else
            {
                BI_CORRUGADOB_V.Color2 = Color.Yellow;
            }

            if (BI_CORRUGADOC_V.SelectColor2 && BI_CORRUGADOC_V.SelectColor3)
            {
                BI_CORRUGADOC_V.Color2 = Color.Green;
            }
            else
            {
                BI_CORRUGADOC_V.Color2 = Color.Yellow;
            }

            if (BI_CORRUGADOD_V.SelectColor2 && BI_CORRUGADOD_V.SelectColor3)
            {
                BI_CORRUGADOD_V.Color2 = Color.Green;
            }
            else
            {
                BI_CORRUGADOD_V.Color2 = Color.Yellow;
            }

            if (BI_CORRUGADOE_V.SelectColor2 && BI_CORRUGADOE_V.SelectColor3)
            {
                BI_CORRUGADOE_V.Color2 = Color.Green;
            }
            else
            {
                BI_CORRUGADOE_V.Color2 = Color.Yellow;
            }

            if (BI_CORRUGADOF_V.SelectColor2 && BI_CORRUGADOF_V.SelectColor3)
            {
                BI_CORRUGADOF_V.Color2 = Color.Green;
            }
            else
            {
                BI_CORRUGADOF_V.Color2 = Color.Yellow;
            }

        }


        private void timer4_Tick(object sender, EventArgs e)
        {
            //comboBox1.SelectedText = Micro800.Read("MODELO_SELECCIONADO");
            listo = true;
            timer4.Stop();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (permiso)
            {
                this.Close();
            }
        }
    }
}
