using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using System.Configuration;
using TalentBookingManagement.ViewModels;

namespace TalentBookingManagement.BookingManagement
{
    /// <summary>
    /// Interaction logic for UpdateBookingWindow.xaml
    /// </summary>
    public partial class UpdateBookingWindow : Window
    {
        public UpdateBookingWindow()
        {
            InitializeComponent();
            DataContext = new UpdateBookingViewModel();
        }

        private void SearchByBookingIDButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BookingIDSearchTextBox.Text, out int bookingID))
            {
                var viewModel = DataContext as UpdateBookingViewModel;
                viewModel?.LoadBookingsByBookingID(bookingID);
            }
        }

        private void SearchByClientIDButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ClientIDSearchTextBox.Text, out int clientID))
            {
                var viewModel = DataContext as UpdateBookingViewModel;
                viewModel?.LoadBookingsByClientID(clientID);
            }
        }

        private void BookingsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as UpdateBookingViewModel;
            viewModel.SelectedBooking = BookingsDataGrid.SelectedItem as Booking;
        }

        private void UpdateBookingButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as UpdateBookingViewModel;
            viewModel?.UpdateBooking();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to navigate back or close the window
            Close();
        }
    }
}
