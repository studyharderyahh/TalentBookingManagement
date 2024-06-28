using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;


namespace TalentBookingManagement.ViewModels
{
    public class ClientDetailsViewModel : BaseViewModel
    {
        public string ClientID { get; set; }
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
        public int PersonID { get; set; }


        public ICommand FetchCommand { get; set; }

        /*public ClientDetailsViewModel()
        {
            FetchCommand = new RelayCommand(ExecuteFetch);
        }*/

        private void ExecuteFetch(object parameter)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ReadClientInfo", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ClientID", ClientID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        FirstName = reader["FirstName"].ToString();
                        LastName = reader["LastName"].ToString();
                        PhoneNumber = reader["PhoneNumber"].ToString();
                        Age = Convert.ToInt32(reader["Age"]);
                        Gender = reader["Gender"].ToString();
                        Email = reader["Email"].ToString();
                        City = reader["City"].ToString();
                        Suburb = reader["Suburb"].ToString();
                        StreetAddress = reader["StreetAddress"].ToString();
                        Postcode = reader["Postcode"].ToString();
                        ActiveStatus = reader["ActiveStatus"].ToString();
                        PersonID = Convert.ToInt32(reader["PersonID"]);
                    }
                    else
                    {
                        MessageBox.Show("Invalid client ID entered, please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

