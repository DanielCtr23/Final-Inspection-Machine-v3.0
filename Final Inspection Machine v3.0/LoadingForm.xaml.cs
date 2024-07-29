using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para LoadingForm.xaml
    /// </summary>
    public partial class LoadingForm : Window
    {
        public LoadingForm( )
        {
            InitializeComponent();

            TB1.Text = "✓ Comunicación Establecida con Base de Datos";
            TB1.FontWeight = FontWeights.Bold;
            TB1.Foreground = System.Windows.Media.Brushes.Green;
        }

        public void Carga1(string Tipo, string IPPLC, string Programa, bool op)
        {
            if (op)
            {
                TB2.Text = "✓ Comunicación Establecida con PLC";
                TB2.FontWeight = FontWeights.Bold;
                TB2.Foreground = System.Windows.Media.Brushes.Green;

                Thread.Sleep(30);

                TB3.Text = "✓ Tipo: " + Tipo;
                TB3.Foreground = System.Windows.Media.Brushes.Black;

                Thread.Sleep(30);

                TB4.Text = "✓ IP: " + IPPLC;
                TB4.Foreground = System.Windows.Media.Brushes.Black;

                Thread.Sleep(30);

                TB5.Text = "✓ Programa: " + Programa;
                TB5.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                TB2.Text = "✘ Comunicación No Establecida con PLC";
                TB2.FontWeight = FontWeights.Bold;
                TB2.Foreground = System.Windows.Media.Brushes.Red;

                Thread.Sleep(30);

                TB3.Text = "✘ Tipo: " + Tipo;
                TB3.Foreground = System.Windows.Media.Brushes.Red;

                Thread.Sleep(30);

                TB4.Text = "✘ IP: " + IPPLC;
                TB4.Foreground = System.Windows.Media.Brushes.Red;

                Thread.Sleep(30);

                TB5.Text = "✘ Programa: " + Programa;
                TB5.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        public void Carga2(string[] Camara, string[] IPCamara, bool op)
        {
            if(op)
            {
                TB6.Text = "✓ Comunicación Establecida con Camaras";
                TB6.FontWeight = FontWeights.Bold;
                TB6.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                TB6.Text = "✘ Comunicación No Establecida con Camaras";
                TB6.FontWeight = FontWeights.Bold;
                TB6.Foreground = System.Windows.Media.Brushes.Red;
            }

            Thread.Sleep(30);

            if (IPCamara[0] != "0")
            {
                TB7.Text = "✓ Camara: " + Camara[0] + " \t IP: " + IPCamara[0];
                TB7.Foreground = System.Windows.Media.Brushes.Black;

            }
            else
            {
                TB7.Text = "✘ Camara: " + Camara[0] + " \t IP: -";
                TB7.Foreground = System.Windows.Media.Brushes.Red;
            }

            Thread.Sleep(30);

            if (IPCamara[1] != "0")
            {
                TB8.Text = "✓ Camara: " + Camara[1] + " \t IP: " + IPCamara[1];
                TB8.Foreground = System.Windows.Media.Brushes.Black;

            }
            else
            {
                TB8.Text = "✘ Camara: " + Camara[1] + " \t IP: -";
                TB8.Foreground = System.Windows.Media.Brushes.Red;
            }

            Thread.Sleep(30);

            if (IPCamara[2] != "0")
            {
                TB9.Text = "✓ Camara: " + Camara[2] + " \t IP: " + IPCamara[2];
                TB9.Foreground = System.Windows.Media.Brushes.Black;

            }
            else
            {
                TB9.Text = "✘ Camara: " + Camara[2] + " \t IP: -";
                TB9.Foreground = System.Windows.Media.Brushes.Red;
            }

            if (IPCamara[3] != "0")
            {
                TB10.Text = "✓ Camara: " + Camara[3] + " \t IP: " + IPCamara[3];
                TB10.Foreground = System.Windows.Media.Brushes.Black;

            }
            else
            {
                TB10.Text = "✘ Camara: " + Camara[3] + " \t IP: -";
                TB10.Foreground = System.Windows.Media.Brushes.Red;
            }

        }

        public void Carga3(string IPEtiquetadora,bool op)
        {
            if (op)
            {
                TB11.Text = "✓ Conexión Establecida con Etiquetadora";
                TB11.FontWeight = FontWeights.Bold;
                TB11.Foreground = System.Windows.Media.Brushes.Green;

                Thread.Sleep(30);

                TB12.Text = "✓ IP: " + IPEtiquetadora;
                TB12.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                TB11.Text = "✘ Conexión No Establecida con Etiquetadora";
                TB11.FontWeight = FontWeights.Bold;
                TB11.Foreground = System.Windows.Media.Brushes.Red;

                Thread.Sleep(30);

                TB12.Text = "✘ IP: -";
                TB12.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Thread.Sleep(1000);
            e.Cancel = false;
        }
    }
}
