using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

namespace TalentBookingManagement.BookingManagement
{
    /// <summary>
    /// Interaction logic for UpdateBookingWindow.xaml
    /// </summary>
    public partial class UpdateBookingWindow : Window
    {
        public ObservableCollection<Booking> Bookings { get; set; }
        string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_YYE_ProjectA;User Id=S2401_Elisa;Password=fBit$98969;";
        public UpdateBookingWindow()
        {
            InitializeComponent();
        }

        private void LoadBookings()
        {
            Bookings = new ObservableCollection<Booking>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Booking";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Bookings.Add(new Booking
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
                            ActiveStatus = reader["ActiveStatus"].ToString()
                        });
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            BookingsListView.ItemsSource = Bookings;
        }

        private void BookingsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsListView.SelectedItem is Booking selectedBooking)
            {
                SpecialRequirementTextBox.Text = selectedBooking.SpecialRequirement;
                ClientIDTextBox.Text = selectedBooking.ClientID.ToString();
                CampaignIDTextBox.Text = selectedBooking.CampaignID.ToString();
                StaffIDTextBox.Text = selectedBooking.StaffID.ToString();
                BookingTimeDatePicker.SelectedDate = selectedBooking.BookingTime;
                CampaignStartDateDatePicker.SelectedDate = selectedBooking.CampaignStartDate;
                CampaignEndDateDatePicker.SelectedDate = selectedBooking.CampaignEndDate;
                CampaignLocationTextBox.Text = selectedBooking.CampaignLocation;
                ActiveStatusTextBox.Text = selectedBooking.ActiveStatus;
            }
        }

        private void UpdateBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsListView.SelectedItem is Booking selectedBooking)
            {
                selectedBooking.SpecialRequirement = SpecialRequirementTextBox.Text;
                selectedBooking.ClientID = int.Parse(ClientIDTextBox.Text);
                selectedBooking.CampaignID = int.Parse(CampaignIDTextBox.Text);
                selectedBooking.StaffID = int.Parse(StaffIDTextBox.Text);
                selectedBooking.BookingTime = BookingTimeDatePicker.SelectedDate.Value;
                selectedBooking.CampaignStartDate = CampaignStartDateDatePicker.SelectedDate.Value;
                selectedBooking.CampaignEndDate = CampaignEndDateDatePicker.SelectedDate.Value;
                selectedBooking.CampaignLocation = CampaignLocationTextBox.Text;
                selectedBooking.ActiveStatus = ActiveStatusTextBox.Text;

                string connectionString = "your_connection_string_here"; // Replace with your actual connection string

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE Booking
                        SET SpecialRequirement = @SpecialRequirement,
                            ClientID = @ClientID,
                            CampaignID = @CampaignID,
                            StaffID = @StaffID,
                            BookingTime = @BookingTime,
                            CampaignStartDate = @CampaignStartDate,
                            CampaignEndDate = @CampaignEndDate,
                            CampaignLocation = @CampaignLocation,
                            ActiveStatus = @ActiveStatus
                        WHERE BookingID = @BookingID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SpecialRequirement", selectedBooking.SpecialRequirement);
                    command.Parameters.AddWithValue("@ClientID", selectedBooking.ClientID);
                    command.Parameters.AddWithValue("@CampaignID", selectedBooking.CampaignID);
                    command.Parameters.AddWithValue("@StaffID", selectedBooking.StaffID);
                    command.Parameters.AddWithValue("@BookingTime", selectedBooking.BookingTime);
                    command.Parameters.AddWithValue("@CampaignStartDate", selectedBooking.CampaignStartDate);
                    command.Parameters.AddWithValue("@CampaignEndDate", selectedBooking.CampaignEndDate);
                    command.Parameters.AddWithValue("@CampaignLocation", selectedBooking.CampaignLocation);
                    command.Parameters.AddWithValue("@ActiveStatus", selectedBooking.ActiveStatus);
                    command.Parameters.AddWithValue("@BookingID", selectedBooking.BookingID);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Booking updated successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to update.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
