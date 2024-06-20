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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.BookingManagement
{
    /// <summary>
    /// Interaction logic for CreateClientWindow.xaml
    /// </summary>
    public partial class CreateClientWindow : Window
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        public Client CreatedClient { get; private set; }

        public CreateClientWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (!ValidateInput())
            {
                return;
            }

            // Gather input values
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phoneNumber = txtPhoneNumber.Text.Trim();
            string email = txtEmail.Text.Trim();
            int age = int.Parse(txtAge.Text.Trim());
            string gender = cmbGender.SelectedItem.ToString();
            string city = txtCity.Text.Trim();
            string suburb = txtSuburb.Text.Trim();
            string streetAddress = txtStreetAddress.Text.Trim();
            string postcode = txtPostcode.Text.Trim();

            // Call database method to add client
            int clientID = CreateClient(firstName, lastName, phoneNumber, email, age, gender, city, suburb, streetAddress, postcode);

            if (clientID > 0)
            {
                // Client creation successful
                CreatedClient = new Client
                {
                    ClientID = clientID,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    Age = age,
                    Gender = gender,
                    City = city,
                    Suburb = suburb,
                    StreetAddress = streetAddress,
                    Postcode = postcode
                };

                this.DialogResult = true; // Close the window with DialogResult true
            }
            else
            {
                // Client creation failed
                MessageBox.Show("Failed to create client. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Indicate cancellation and close the window
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                ShowValidationError("Please enter the first name.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                ShowValidationError("Please enter the last name.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                ShowValidationError("Please enter the phone number.");
                return false;
            }

            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                ShowValidationError("Please enter a valid email address.");
                return false;
            }

            int age;
            if (!int.TryParse(txtAge.Text.Trim(), out age) || age <= 0 || age > 150)
            {
                ShowValidationError("Please enter a valid age between 1 and 150.");
                return false;
            }

            if (cmbGender.SelectedItem == null)
            {
                ShowValidationError("Please select a gender.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                ShowValidationError("Please enter the city.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSuburb.Text))
            {
                ShowValidationError("Please enter the suburb.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStreetAddress.Text))
            {
                ShowValidationError("Please enter the street address.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPostcode.Text) || !IsValidPostcode(txtPostcode.Text.Trim()))
            {
                ShowValidationError("Please enter a valid postcode.");
                return false;
            }

            // All validations passed
            return true;
        }

        private int CreateClient(string firstName, string lastName, string phoneNumber, string email, int age,
                                 string gender, string city, string suburb, string streetAddress, string postcode)
        {
            try
            {
                int clientID = 0;

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

                        // Output parameter for new client ID
                        SqlParameter clientIDParameter = new SqlParameter("@ClientID", SqlDbType.Int);
                        clientIDParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(clientIDParameter);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Retrieve the new client ID from output parameter
                        clientID = (int)clientIDParameter.Value;

                        connection.Close();
                    }
                }

                return clientID; // Success
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // Error
            }
        }

        private void ClearFields()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAge.Text = string.Empty;
            cmbGender.SelectedItem = null;
            txtCity.Text = string.Empty;
            txtSuburb.Text = string.Empty;
            txtStreetAddress.Text = string.Empty;
            txtPostcode.Text = string.Empty;
        }

        private void ShowValidationError(string message)
        {
            MessageBox.Show(message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool IsValidEmail(string email)
        {
            // Using a simple regular expression for email validation
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPostcode(string postcode)
        {
            // Using a simple regular expression for postcode validation
            string pattern = @"^\d{4,5}$";
            return Regex.IsMatch(postcode, pattern);
        }
    }
}
