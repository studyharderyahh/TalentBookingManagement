using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class UpdateBookingViewModel : INotifyPropertyChanged
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public ObservableCollection<Booking> Bookings { get; set; }

        private Booking _selectedBooking;
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
            }
        }

        public UpdateBookingViewModel()
        {
            Bookings = new ObservableCollection<Booking>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadBookingsByClientID(int clientID)
        {
            LoadBookings("spGetBookingsByClientID", new SqlParameter("@ClientID", clientID));
        }

        public void LoadBookingsByBookingID(int bookingID)
        {
            LoadBookings("spGetBookingsByBookingID", new SqlParameter("@BookingID", bookingID));
        }

        private void LoadBookings(string storedProcedureName, SqlParameter parameter)
        {
            Bookings.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(storedProcedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(parameter);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Booking booking = new Booking
                        {
                            BookingID = (int)reader["BookingID"],
                            SpecialRequirement = reader["SpecialRequirement"].ToString(),
                            ClientID = (int)reader["ClientID"],
                            CampaignID = (int)reader["CampaignID"],
                            StaffID = (int)reader["StaffID"],
                            BookingTime = DateTime.Parse(reader["BookingTime"].ToString()),
                            CampaignStartDate = DateTime.Parse(reader["CampaignStartDate"].ToString()),
                            CampaignEndDate = DateTime.Parse(reader["CampaignEndDate"].ToString()),
                            CampaignLocation = reader["CampaignLocation"].ToString(),
                            RateType = reader["RateType"].ToString(),
                            Duration = (int)reader["Duration"],
                            BookingFee = (decimal)reader["BookingFee"],
                            ActiveStatus = reader["ActiveStatus"].ToString()
                        };

                        string talentIDs = reader["TalentIDs"].ToString();
                        if (!string.IsNullOrEmpty(talentIDs))
                        {
                            string[] talentIDsArray = talentIDs.Split(',');
                            foreach (string talentID in talentIDsArray)
                            {
                                if (int.TryParse(talentID, out int parsedTalentID))
                                {
                                    booking.TalentIDs.Add(parsedTalentID);
                                }
                            }
                        }

                        Bookings.Add(booking);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }




        public void UpdateBooking()
        {
            if (SelectedBooking == null)
            {
                MessageBox.Show("Please select a booking to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("spUpdateBooking", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@BookingID", SelectedBooking.BookingID);
                command.Parameters.AddWithValue("@SpecialRequirement", SelectedBooking.SpecialRequirement);
                command.Parameters.AddWithValue("@ClientID", SelectedBooking.ClientID);
                command.Parameters.AddWithValue("@CampaignID", SelectedBooking.CampaignID);
                command.Parameters.AddWithValue("@StaffID", SelectedBooking.StaffID);
                command.Parameters.AddWithValue("@BookingTime", SelectedBooking.BookingTime);
                command.Parameters.AddWithValue("@CampaignStartDate", SelectedBooking.CampaignStartDate);
                command.Parameters.AddWithValue("@CampaignEndDate", SelectedBooking.CampaignEndDate);
                command.Parameters.AddWithValue("@CampaignLocation", SelectedBooking.CampaignLocation);
                command.Parameters.AddWithValue("@RateType", SelectedBooking.RateType);
                command.Parameters.AddWithValue("@Duration", SelectedBooking.Duration);
                command.Parameters.AddWithValue("@BookingFee", SelectedBooking.BookingFee);
                command.Parameters.AddWithValue("@ActiveStatus", SelectedBooking.ActiveStatus);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Booking updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadBookingsByClientID(SelectedBooking.ClientID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
