using Final_Inspection_Machine_v3._0.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    /// Lógica de interacción para Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private Dictionary<string, Page> pages;
        public Dashboard()
        {
            InitializeComponent();
            pages = new Dictionary<string, Page>
            {
                { "Page 1", new Inicio() },
            };
            Frame.NavigationService.Content = pages;
        }

        private void InicioBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.NavigationService.Navigate(new Inicio());
            SeleccionBtn(InicioBtn);
        }

        private void SeleccionBtn(Button button)
        {
            InicioBtn.Background = new SolidColorBrush(Color.FromArgb(255,67,67,67 ));
            DetalleBtn.Background = new SolidColorBrush(Color.FromArgb(255, 67, 67, 67));
            InformesBtn.Background = new SolidColorBrush(Color.FromArgb(255, 67, 67, 67));
            ContadoresBtn.Background = new SolidColorBrush(Color.FromArgb(255, 67, 67, 67));

            button.Background = new SolidColorBrush(Colors.DarkSlateGray);
        }

        private void DetalleBtn_Click(object sender, RoutedEventArgs e)
        {
            SeleccionBtn(DetalleBtn);
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
