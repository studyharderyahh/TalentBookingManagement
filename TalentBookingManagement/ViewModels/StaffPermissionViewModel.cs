using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class StaffPermissionViewModel : BaseViewModel
    {
        private int staffID;
        private ObservableCollection<StaffPermission> staffPermissions;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public int StaffID
        {
            get { return staffID; }
            set
            {
                staffID = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StaffPermission> StaffPermissions
        {
            get { return staffPermissions; }
            set
            {
                staffPermissions = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadPermissionsCommand { get; }

        /*public StaffPermissionViewModel()
        {
            StaffPermissions = new ObservableCollection<StaffPermission>();
            LoadPermissionsCommand = new RelayCommand(ExecuteLoad);
        }*/

        private void ExecuteLoad(object parameter)
        {
            StaffPermissions.Clear();

            if (StaffID <= 0)
            {
                MessageBox.Show("Invalid Staff ID entered, please enter a valid Staff ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("ViewStaffPermission", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StaffID", StaffID);

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