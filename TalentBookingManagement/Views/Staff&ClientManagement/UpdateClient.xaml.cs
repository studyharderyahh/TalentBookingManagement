using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using TalentBookingManagement.Models;
using System.Configuration;

namespace TalentBookingManagement.Views.Staff_ClientManagement
{
    public partial class UpdateClient : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public UpdateClient()
        {
            InitializeComponent();
        }

        private void LoadClientDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ClientIDInputTextBox.Text, out int clientId))
            {
                LoadClientDetails(clientId);
            }
            else
            {
                MessageBox.Show("Please enter a valid Client ID.");
            }
        }
        private void LoadClientDetails(int clientId)
        {

            Client client = GetClientFromDatabase(clientId);

            if (client != null)
            {
                ClientIDTextBox.Text = client.ClientID.ToString();
                FirstNameTextBox.Text = client.FirstName;
                LastNameTextBox.Text = client.LastName;
                PhoneNumberTextBox.Text = client.PhoneNumber;
                AgeTextBox.Text = client.Age.ToString();
                GenderTextBox.Text = client.Gender;
                EmailTextBox.Text = client.Email;
                CityTextBox.Text = client.City;
                SuburbTextBox.Text = client.Suburb;
                StreetAddressTextBox.Text = client.StreetAddress;
                PostcodeTextBox.Text = client.Postcode.ToString();
                ActiveStatusTextBox.Text = client.ActiveStatus;
                PersonIDTextBox.Text = client.PersonID.ToString();

                ClientDetailsStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Client not found.");
            }
        }
        private Client GetClientFromDatabase(int clientId)
        {
            Client client = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("ReadClientInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ClientID", clientId);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                client = new Client
                                {
                                    ClientID = Convert.ToInt32(reader["ClientID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Age = Convert.ToInt32(reader["Age"]),
                                    Gender = reader["Gender"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    City = reader["City"].ToString(),
                                    Suburb = reader["Suburb"].ToString(),
                                    StreetAddress = reader["StreetAddress"].ToString(),
                                    Postcode = reader["Postcode"].ToString(),
                                    ActiveStatus = reader["ActiveStatus"].ToString(),
                                    PersonID = Convert.ToInt32(reader["PersonID"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            return client;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ClientIDTextBox.Text, out int clientId) &&
                int.TryParse(PersonIDTextBox.Text, out int personId))
            {
                Client client = new Client
                {
                    ClientID = clientId,
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    PhoneNumber = PhoneNumberTextBox.Text,
                    Age = int.Parse(AgeTextBox.Text),
                    Gender = GenderTextBox.Text,
                    Email = EmailTextBox.Text,
                    City = CityTextBox.Text,
                    Suburb = SuburbTextBox.Text,
                    StreetAddress = StreetAddressTextBox.Text,
                    Postcode = PostcodeTextBox.Text,
                    ActiveStatus = ActiveStatusTextBox.Text,
                    PersonID = personId,
                };

                if (client.ActiveStatus == "Active")
                {
                    bool isUpdated = UpdateClientInDatabase(client);

                    if (isUpdated)
                    {
                        MessageBox.Show("Client details updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update client details.");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot update inactive client details.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid client ID.");
            }
        }

        private bool UpdateClientInDatabase(Client client)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("UpdateClientInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ClientID", client.ClientID);
                        command.Parameters.AddWithValue("@PersonID", client.PersonID);
                        command.Parameters.AddWithValue("@FirstName", client.FirstName);
                        command.Parameters.AddWithValue("@LastName", client.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", client.PhoneNumber);
                        command.Parameters.AddWithValue("@Age", client.Age);
                        command.Parameters.AddWithValue("@Gender", client.Gender);
                        command.Parameters.AddWithValue("@Email", client.Email);
                        command.Parameters.AddWithValue("@City", client.City);
                        command.Parameters.AddWithValue("@Suburb", client.Suburb);
                        command.Parameters.AddWithValue("@StreetAddress", client.StreetAddress);
                        command.Parameters.AddWithValue("@Postcode", client.Postcode);
                        command.Parameters.AddWithValue("@ActiveStatus", client.ActiveStatus);

                        connection.Open();
                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}