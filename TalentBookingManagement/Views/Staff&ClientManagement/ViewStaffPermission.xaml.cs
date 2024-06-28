using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.Views.Staff_ClientManagement
{
    /// <summary>
    /// Interaction logic for ViewStaffPermission.xaml
    /// </summary>
    public partial class ViewStaffPermission : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        private ObservableCollection<StaffPermission> StaffPermissions { get; set; }

        public ViewStaffPermission()
        {
            InitializeComponent();
            StaffPermissions = new ObservableCollection<StaffPermission>();
            StaffPermissionsDataGrid.ItemsSource = StaffPermissions;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StaffPermissions.Clear();
            if (int.TryParse(StaffIDTextBox.Text, out int staffID) && staffID > 0)
            {
                LoadPermissions(staffID);
            }
            else
            {
                MessageBox.Show("Invalid Staff ID entered, please enter a valid Staff ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPermissions(int staffID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("ViewStaffPermission", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StaffID", staffID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StaffPermissions.Add(new StaffPermission
                                    {
                                        StaffID = reader.GetInt32(reader.GetOrdinal("StaffID")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                                        TypeOfPermission = reader.GetString(reader.GetOrdinal("TypeOfPermission"))
                                    });
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Staff ID entered, please enter a valid Staff ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
