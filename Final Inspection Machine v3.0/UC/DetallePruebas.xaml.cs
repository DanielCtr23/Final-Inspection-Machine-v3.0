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
using System.Windows.Threading;
using System.Windows.Controls.Primitives;


namespace Final_Inspection_Machine_v3._0.UC
{
    /// <summary>
    /// Lógica de interacción para DetallePruebas.xaml
    /// </summary>
    public partial class DetallePruebas : UserControl
    {
        DataManager DM = new DataManager();
        private DispatcherTimer BusquedaTmr;
        public DetallePruebas()
        {
            InitializeComponent();
            BusquedaTmr = new DispatcherTimer();
            BusquedaTmr.Interval = TimeSpan.FromSeconds(10);
            BusquedaTmr.Tick += BusquedaTmr_Tick;
            PruebasDG.ItemsSource = DM.Detalles(null, null, null, null, null, true, true).DefaultView;
            ModelosCB.ItemsSource = DM.ObtenerModelos().DefaultView;
            ModelosCB.DisplayMemberPath = "PN";
            ModelosCB.SelectedValuePath = "idmodelos";

            
        }

        private void PART_Button_Click(object sender, RoutedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                // Obtener el Popup asociado al DatePicker
                Popup popup = GetDatePickerPopup(datePicker);

                // Mostrar u ocultar el Popup
                if (popup != null)
                {
                    popup.IsOpen = !popup.IsOpen; // Alternar la visibilidad del Popup
                }
            }
        }

        // Método para obtener el Popup asociado al DatePicker
        private Popup GetDatePickerPopup(DatePicker datePicker)
        {
            if (datePicker.Template.FindName("PART_Popup", datePicker) is Popup popup)
            {
                return popup;
            }
            return null;
        }

        // Método para obtener el calendario asociado al DatePicker
        private Calendar GetDatePickerCalendar(DatePicker datePicker)
        {
            Calendar calendar = null;
            if (datePicker.Template.FindName("PART_Popup", datePicker) is Popup popup)
            {
                if (popup.Child is Border border)
                {
                    if (border.Child is Grid grid)
                    {
                        calendar = grid.Children
                            .OfType<Calendar>()
                            .FirstOrDefault();
                    }
                }
            }
            return calendar;
        }
        private void PART_Button_Click1(object sender, RoutedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                // Toggle IsDropDownOpen to open/close the DatePicker calendar
                if (datePicker.IsDropDownOpen)
                {
                    
                    datePicker.IsDropDownOpen = true;
                    
                }
                else
                {
                    datePicker.IsDropDownOpen = true;
                }
            }
        }

        private void BusquedaTmr_Tick(object sender, EventArgs e)
        {
            BusquedaTmr?.Stop();
            MensajeTB.Text = "Actualizando...";
            CargarTabla();
        }

        private void CargarTabla()
        {
            int? modelo, estacion;
            if (ModelosCB.SelectedValue == null)
            {
                modelo = null;
            }
            else
            {
                modelo = int.Parse(ModelosCB.SelectedValue.ToString());
            }
            if (EstacionCB.SelectedValue == null)
            {
                estacion = null;
            }
            else
            {
                estacion = int.Parse(EstacionCB.SelectedValue.ToString());
            }
            PruebasDG.ItemsSource = DM.Detalles(BuscadorTB.Text, modelo, estacion,
                InicioDTB.SelectedDate, FinDTB.SelectedDate, PassCB.IsChecked, FailCB.IsChecked).DefaultView;
            MensajeTB.Text = DateTime.Now.ToString() + " : " + PruebasDG.Items.Count.ToString() + " Registros Cargados (Límite 1000)";
        }

        private void BuscadorTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void ModelosCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void EstacionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void PassCB_Checked(object sender, RoutedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void PassCB_Unchecked(object sender, RoutedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void InicioDTB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void FinDTB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void FailCB_Unchecked(object sender, RoutedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void FailCB_Checked(object sender, RoutedEventArgs e)
        {
            BusquedaTmr.Stop();
            BusquedaTmr.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BusquedaTmr.Stop();
            BuscadorTB.Text = "";
            EstacionCB.SelectedIndex = -1;
            ModelosCB.SelectedIndex = -1;
            InicioDTB.SelectedDate = null;
            FinDTB.SelectedDate = null;
            PassCB.IsChecked = false;
            FailCB.IsChecked = false;

            BusquedaTmr.Stop();
        }

        private void PruebasDG_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataRowView valor = PruebasDG.SelectedItem as DataRowView;
            if (e.AddedCells.Count>0)
            {
                try
                {
                    int valor1 = int.Parse(valor.Row[0].ToString());
                    Detalle.CargarDetalle(valor1);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
