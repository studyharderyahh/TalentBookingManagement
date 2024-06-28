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
using System.Windows.Shapes;
using TalentBookingManagement.ViewModels;

namespace TalentBookingManagement.Views
{
    /// <summary>
    /// Interaction logic for CancelBookingWindow.xaml
    /// </summary>
    public partial class CancelBookingWindow : Window
    {
        public CancelBookingWindow()
        {
            InitializeComponent();
            DataContext = new CancelBookingViewModel();
        }

        private void SelectedOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (sender as ComboBox)?.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedOption))
            {
                var viewModel = DataContext as CancelBookingViewModel;
                viewModel.SelectedOption = selectedOption;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
