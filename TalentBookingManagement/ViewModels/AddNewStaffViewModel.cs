using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class AddNewStaffViewModel : BaseViewModel
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        // Properties for staff details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Suburb { get; set; }
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }
        public string ActiveStatus { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }

        // Command for saving staff
        public ICommand SaveCommand { get; set; }

        /*public AddNewStaffViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave);
        }*/


        private void ExecuteSave(object parameter)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddNewStaff", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                command.Parameters.AddWithValue("@Age", Age);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@City", City);
                command.Parameters.AddWithValue("@Suburb", Suburb);
                command.Parameters.AddWithValue("@StreetAddress", StreetAddress);
                command.Parameters.AddWithValue("@Postcode", Postcode);
                command.Parameters.AddWithValue("@ActiveStatus", ActiveStatus);
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@RoleID", RoleID);

                // Add an output parameter for returning the StaffID
                SqlParameter outputParameter = new SqlParameter();
                outputParameter.ParameterName = "@StaffID";
                outputParameter.SqlDbType = SqlDbType.Int;
                outputParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputParameter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    int newStaffId = (int)command.Parameters["@StaffID"].Value;

                    MessageBox.Show($"New staff added with ID: {newStaffId}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

    }
}
