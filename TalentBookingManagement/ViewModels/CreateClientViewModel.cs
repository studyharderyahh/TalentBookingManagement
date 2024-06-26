using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class CreateClientViewModel : BaseViewModel
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string age;
        private string gender;
        private string city;
        private string suburb;
        private string streetAddress;
        private string postcode;

        public CreateClientViewModel()
        {
            CreateCommand = new RelayCommand(CreateClient, CanCreateClient);
        }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value, nameof(FirstName));
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value, nameof(LastName));
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value, nameof(PhoneNumber));
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value, nameof(Email));
        }

        public string Age
        {
            get => age;
            set => SetProperty(ref age, value, nameof(Age));
        }

        public string Gender
        {
            get => gender;
            set => SetProperty(ref gender, value, nameof(Gender));
        }

        public string City
        {
            get => city;
            set => SetProperty(ref city, value, nameof(City));
        }

        public string Suburb
        {
            get => suburb;
            set => SetProperty(ref suburb, value, nameof(Suburb));
        }

        public string StreetAddress
        {
            get => streetAddress;
            set => SetProperty(ref streetAddress, value, nameof(StreetAddress));
        }

        public string Postcode
        {
            get => postcode;
            set => SetProperty(ref postcode, value, nameof(Postcode));
        }

        public ICommand CreateCommand { get; }

        private bool CanCreateClient()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   IsValidEmail(Email) &&
                   int.TryParse(Age, out int ageResult) && ageResult > 0 && ageResult <= 150 &&
                   !string.IsNullOrWhiteSpace(Gender) &&
                   !string.IsNullOrWhiteSpace(City) &&
                   !string.IsNullOrWhiteSpace(Suburb) &&
                   !string.IsNullOrWhiteSpace(StreetAddress) &&
                   IsValidPostcode(Postcode);
        }

        private void CreateClient()
        {
            try
            {
                int clientID = AddClientToDatabase(FirstName, LastName, PhoneNumber, Email, int.Parse(Age), Gender, City, Suburb, StreetAddress, Postcode);
                if (clientID > 0)
                {
                    MessageBox.Show($"Client created successfully. Client ID: {clientID}", "Create Client", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to create client. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int AddClientToDatabase(string firstName, string lastName, string phoneNumber, string email, int age, string gender, string city, string suburb, string streetAddress, string postcode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("AddNewClient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@Suburb", suburb);
                    command.Parameters.AddWithValue("@StreetAddress", streetAddress);
                    command.Parameters.AddWithValue("@Postcode", postcode);
                    command.Parameters.AddWithValue("@ActiveStatus", "Active");

                    SqlParameter clientIDParameter = new SqlParameter("@ClientID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(clientIDParameter);

                    connection.Open();
                    command.ExecuteNonQuery();

                    return (int)clientIDParameter.Value;
                }
            }
        }

        private void ClearFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            Age = string.Empty;
            Gender = string.Empty;
            City = string.Empty;
            Suburb = string.Empty;
            StreetAddress = string.Empty;
            Postcode = string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return false;
            }

            string pattern = @"^\d{4,5}$";
            return Regex.IsMatch(postcode, pattern);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            ((RelayCommand)CreateCommand).RaiseCanExecuteChanged();
        }
    }
}
