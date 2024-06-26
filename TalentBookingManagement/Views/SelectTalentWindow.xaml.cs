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

        public SelectTalentWindow()
        {
            InitializeComponent();
            viewModel = new SelectTalentViewModel();
            viewModel.CloseRequested += () => this.Close();
            this.DataContext = viewModel;
        }

        public ObservableCollection<Talent> SelectedTalents => viewModel.SelectedTalents;
    }
}
