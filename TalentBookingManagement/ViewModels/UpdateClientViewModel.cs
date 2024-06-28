using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class UpdateClientViewModel : INotifyPropertyChanged
    {
        private Client _client;

        public Client Client
        {
            get { return _client; }
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadClientCommand { get; private set; }
        public ICommand UpdateClientCommand { get; private set; }

        /*public UpdateClientViewModel()
        {
            LoadClientCommand = new RelayCommand(ExecuteLoad);
            UpdateClientCommand = new RelayCommand(ExecuteUpdate, CanUpdateClient);
        }*/

        private void ExecuteLoad(object parameter)
        {
            if (parameter is int clientId)
            {
                Client = GetClientFromDatabase(clientId);
            }
            else
            {
                MessageBox.Show("Please enter a valid Client ID.");
            }
        }

        private Client GetClientFromDatabase(int clientId)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";
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

        private bool CanUpdateClient(object parameter)
        {
            return Client != null && Client.ActiveStatus == "Active";
        }

        private void ExecuteUpdate(object parameter)
        {
            if (Client == null)
            {
                MessageBox.Show("No client selected.");
                return;
            }

            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UpdateClientInfo", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ClientID", Client.ClientID);
                command.Parameters.AddWithValue("@PersonID", Client.PersonID);
                command.Parameters.AddWithValue("@FirstName", Client.FirstName);
                command.Parameters.AddWithValue("@LastName", Client.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", Client.PhoneNumber);
                command.Parameters.AddWithValue("@Age", Client.Age);
                command.Parameters.AddWithValue("@Gender", Client.Gender);
                command.Parameters.AddWithValue("@Email", Client.Email);
                command.Parameters.AddWithValue("@City", Client.City);
                command.Parameters.AddWithValue("@Suburb", Client.Suburb);
                command.Parameters.AddWithValue("@StreetAddress", Client.StreetAddress);
                command.Parameters.AddWithValue("@Postcode", Client.Postcode);
                command.Parameters.AddWithValue("@ActiveStatus", Client.ActiveStatus);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Client information updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


