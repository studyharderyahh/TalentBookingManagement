using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class AddNewClientViewModel : BaseViewModel
    {
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged(); }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
        }

        private string _suburb;
        public string Suburb
        {
            get { return _suburb; }
            set { _suburb = value; OnPropertyChanged(); }
        }

        private string _streetAddress;
        public string StreetAddress
        {
            get { return _streetAddress; }
            set { _streetAddress = value; OnPropertyChanged(); }
        }

        private string _postcode;
        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; OnPropertyChanged(); }
        }

        private string _activeStatus;
        public string ActiveStatus
        {
            get { return _activeStatus; }
            set { _activeStatus = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        /*public AddNewClientViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanSaveClient);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }*/

        private bool CanSaveClient(object parameter) =>
            // Implement validation logic here
            true; // For simplicity, always return true

        private void ExecuteSave(object parameter)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddNewClient", connection);
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

                // Output parameters to retrieve newly inserted IDs
                SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.Int);
                clientIdParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(clientIdParameter);

                SqlParameter personIdParameter = new SqlParameter("@PersonID", SqlDbType.Int);
                personIdParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(personIdParameter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    int newClientId = (int)clientIdParameter.Value;
                    int newPersonId = (int)personIdParameter.Value;

                    MessageBox.Show($"New client added with ID: {newClientId} and PersonID: {newPersonId}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void ExecuteCancel(object parameter)
        {
            // Implement cancel logic if needed
            MessageBox.Show("Operation canceled.");
        }

    }
}
