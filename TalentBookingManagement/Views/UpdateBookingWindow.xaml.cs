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
using TalentBookingManagement.ViewModels;

namespace TalentBookingManagement
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

        private void BookingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            Close();
        }
    }
}
