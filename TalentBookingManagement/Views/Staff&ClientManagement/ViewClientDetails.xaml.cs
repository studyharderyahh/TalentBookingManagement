using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.Views.Staff_ClientManagement
{
    public partial class ViewClientDetails : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public ViewClientDetails()
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
    }
}
