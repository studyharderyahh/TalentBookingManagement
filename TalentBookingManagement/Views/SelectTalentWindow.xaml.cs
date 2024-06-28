using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using TalentBookingManagement.Models;
using TalentBookingManagement.ViewModels;
using System.Collections.ObjectModel;

namespace TalentBookingManagement
{
    /// <summary>
    /// Interaction logic for SelectTalentWindow.xaml
    /// </summary>
    public partial class SelectTalentWindow : Window
    {
        private readonly SelectTalentViewModel viewModel;

        public ObservableCollection<Talent> SelectedTalents { get; private set; } = new ObservableCollection<Talent>();

        public SelectTalentWindow()
        {
            InitializeComponent();
            viewModel = new SelectTalentViewModel();
            viewModel.CloseRequested += ViewModel_CloseRequested;
            this.DataContext = viewModel;
        }

        private void AvailabilityStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAvailabilityStatusCombo = sender as ComboBox;
            if (selectedAvailabilityStatusCombo?.SelectedItem is string selectedAvailabilityStatus)
            {
                // Handle the selected availability status here
                Console.WriteLine($"Selected availability status: {selectedAvailabilityStatus}");
            }
        }
        private void ViewModel_CloseRequested()
        {
            var viewModel = DataContext as SelectTalentViewModel;
            if (viewModel != null)
            {
                SelectedTalents = new ObservableCollection<Talent>(viewModel.SelectedTalents);
                DialogResult = true;
                Close();
            }
        }

    }
}
