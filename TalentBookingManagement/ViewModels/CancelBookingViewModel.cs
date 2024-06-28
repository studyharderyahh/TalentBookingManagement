using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace TalentBookingManagement.ViewModels
{
    public class CancelBookingViewModel : BaseViewModel
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        private string _selectedOption;
        private int _id;
        private ObservableCollection<Booking> _bookings;

        public CancelBookingViewModel()
        {
            SelectedOptions = new ObservableCollection<string> { "BookingID", "ClientID" };
            Bookings = new ObservableCollection<Booking>();
            FetchBookingsCommand = new RelayCommand(FetchBookings);
            CancelBookingCommand = new RelayCommand(CancelBooking);
        }

        public ObservableCollection<string> SelectedOptions { get; }
        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }

        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        public ICommand FetchBookingsCommand { get; }
        public ICommand CancelBookingCommand { get; }
        public ICommand CloseWindowCommand { get; }

        private void FetchBookings()
        {
            Bookings.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("FetchBookingsByBookingIDOrClientID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    if (SelectedOption == "BookingID")
                    {
                        command.Parameters.AddWithValue("@BookingID", ID);
                        command.Parameters.AddWithValue("@ClientID", DBNull.Value);
                    }
                    else if (SelectedOption == "ClientID")
                    {
                        command.Parameters.AddWithValue("@BookingID", DBNull.Value);
                        command.Parameters.AddWithValue("@ClientID", ID);
                    }

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
                            BookingTime = (DateTime)reader["BookingTime"],
                            CampaignStartDate = (DateTime)reader["CampaignStartDate"],
                            CampaignEndDate = (DateTime)reader["CampaignEndDate"],
                            CampaignLocation = reader["CampaignLocation"].ToString(),
                            ActiveStatus = reader["ActiveStatus"].ToString(),
                            TalentIDs = new List<int>()
                        };

                        // Fetch and populate TalentIDs
                        if (!reader.IsDBNull(reader.GetOrdinal("TalentID")))
                        {
                            int talentID = (int)reader["TalentID"];
                            booking.TalentIDs.Add(talentID);
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

        private void CancelBooking()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("CancelBooking", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    if (SelectedOption == "BookingID")
                    {
                        command.Parameters.AddWithValue("@BookingID", ID);
                        command.Parameters.AddWithValue("@ClientID", DBNull.Value);
                    }
                    else if (SelectedOption == "ClientID")
                    {
                        command.Parameters.AddWithValue("@BookingID", DBNull.Value);
                        command.Parameters.AddWithValue("@ClientID", ID);
                    }

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Booking canceled successfully.");
                        FetchBookings(); // Refresh the booking list
                    }
                    else
                    {
                        MessageBox.Show("Failed to cancel booking.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
