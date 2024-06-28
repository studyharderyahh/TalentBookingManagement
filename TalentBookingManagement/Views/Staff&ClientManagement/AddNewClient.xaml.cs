using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.Views.Staff_ClientManagement
{
    public partial class AddNewClient : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public AddNewClient()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddNewClient", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Input parameters
                command.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                command.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text);
                command.Parameters.AddWithValue("@Age", int.Parse(AgeTextBox.Text)); // Assuming AgeTextBox.Text is a valid integer
                command.Parameters.AddWithValue("@Gender", (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                command.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                command.Parameters.AddWithValue("@City", CityTextBox.Text);
                command.Parameters.AddWithValue("@Suburb", SuburbTextBox.Text);
                command.Parameters.AddWithValue("@StreetAddress", StreetAddressTextBox.Text);
                command.Parameters.AddWithValue("@Postcode", PostcodeTextBox.Text);
                command.Parameters.AddWithValue("@ActiveStatus", (ActiveStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());

                // Output parameter for returning the ClientID
                SqlParameter outputParameter = new SqlParameter();
                outputParameter.ParameterName = "@ClientID";
                outputParameter.SqlDbType = SqlDbType.Int;
                outputParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputParameter);

                // Output parameter for returning the PersonID
                SqlParameter outputPersonIDParameter = new SqlParameter();
                outputPersonIDParameter.ParameterName = "@PersonID";
                outputPersonIDParameter.SqlDbType = SqlDbType.Int;
                outputPersonIDParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputPersonIDParameter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve the output parameter values
                    int newClientId = (int)command.Parameters["@ClientID"].Value;
                    int newPersonId = (int)command.Parameters["@PersonID"].Value;

                    MessageBox.Show($"New client added with ID: {newClientId} and PersonID: {newPersonId}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


