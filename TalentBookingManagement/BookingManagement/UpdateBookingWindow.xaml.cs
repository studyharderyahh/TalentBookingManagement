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
using System.Configuration;

namespace TalentBookingManagement.BookingManagement
{
    /// <summary>
    /// Interaction logic for UpdateBookingWindow.xaml
    /// </summary>
    public partial class UpdateBookingWindow : Window
    {
        public ObservableCollection<Booking> Bookings { get; set; }
        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        public UpdateBookingWindow()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings(string query = "SELECT * FROM Booking")
        {
            Bookings = new ObservableCollection<Booking>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Update the Booking instantiation to include new fields
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

                        // Handling TalentIDs - assuming they are stored as comma-separated values in the database
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

            BookingsDataGrid.ItemsSource = Bookings;
        }

        private void BookingsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem is Booking selectedBooking)
            {
                SpecialRequirementTextBox.Text = selectedBooking.SpecialRequirement;
                ClientIDTextBox.Text = selectedBooking.ClientID.ToString();
                CampaignIDTextBox.Text = selectedBooking.CampaignID.ToString();
                StaffIDTextBox.Text = selectedBooking.StaffID.ToString();
                BookingTimeDatePicker.SelectedDate = selectedBooking.BookingTime;
                CampaignStartDateDatePicker.SelectedDate = selectedBooking.CampaignStartDate;
                CampaignEndDateDatePicker.SelectedDate = selectedBooking.CampaignEndDate;
                CampaignLocationTextBox.Text = selectedBooking.CampaignLocation;
                RateTypeTextBox.Text = selectedBooking.RateType; // Bind RateType
                DurationTextBox.Text = selectedBooking.Duration.ToString(); // Bind Duration
                BookingFeeTextBox.Text = selectedBooking.BookingFee.ToString(); // Bind BookingFee
                ActiveStatusTextBox.Text = selectedBooking.ActiveStatus;

                // Bind TalentIDsListBox - assuming ListBox.ItemsSource is bound to TalentIDs
                TalentIDsListBox.ItemsSource = selectedBooking.TalentIDs;
            }
        }

        private void SearchByBookingIDButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BookingIDSearchTextBox.Text, out int bookingID))
            {
                string query = $"SELECT * FROM Booking WHERE BookingID = {bookingID}";
                LoadBookings(query);
            }
            else
            {
                MessageBox.Show("Please enter a valid BookingID.");
            }
        }

        private void SearchByClientIDButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ClientIDSearchTextBox.Text, out int clientID))
            {
                string query = $"SELECT * FROM Booking WHERE ClientID = {clientID}";
                LoadBookings(query);
            }
            else
            {
                MessageBox.Show("Please enter a valid ClientID.");
            }
        }

        private void UpdateBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem is Booking selectedBooking)
            {
                try
                {
                    // Get updated values from input fields
                    string specialRequirement = SpecialRequirementTextBox.Text;
                    int clientID = int.Parse(ClientIDTextBox.Text);
                    int campaignID = int.Parse(CampaignIDTextBox.Text);
                    int staffID = int.Parse(StaffIDTextBox.Text);
                    DateTime bookingTime = BookingTimeDatePicker.SelectedDate.Value;
                    DateTime campaignStartDate = CampaignStartDateDatePicker.SelectedDate.Value;
                    DateTime campaignEndDate = CampaignEndDateDatePicker.SelectedDate.Value;
                    string campaignLocation = CampaignLocationTextBox.Text;
                    string rateType = RateTypeTextBox.Text; // Updated to include RateType
                    int duration = int.Parse(DurationTextBox.Text); // Updated to include Duration
                    decimal bookingFee = decimal.Parse(BookingFeeTextBox.Text); // Updated to include BookingFee
                    string activeStatus = ActiveStatusTextBox.Text;

                    // Check if there are any changes
                    if (specialRequirement == selectedBooking.SpecialRequirement &&
                        clientID == selectedBooking.ClientID &&
                        campaignID == selectedBooking.CampaignID &&
                        staffID == selectedBooking.StaffID &&
                        bookingTime == selectedBooking.BookingTime &&
                        campaignStartDate == selectedBooking.CampaignStartDate &&
                        campaignEndDate == selectedBooking.CampaignEndDate &&
                        campaignLocation == selectedBooking.CampaignLocation &&
                        rateType == selectedBooking.RateType &&
                        duration == selectedBooking.Duration &&
                        bookingFee == selectedBooking.BookingFee &&
                        activeStatus == selectedBooking.ActiveStatus)
                    {
                        MessageBox.Show("No changes detected. Please modify the fields before updating.", "No Changes", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    // Update the selected booking
                    selectedBooking.SpecialRequirement = specialRequirement;
                    selectedBooking.ClientID = clientID;
                    selectedBooking.CampaignID = campaignID;
                    selectedBooking.StaffID = staffID;
                    selectedBooking.BookingTime = bookingTime;
                    selectedBooking.CampaignStartDate = campaignStartDate;
                    selectedBooking.CampaignEndDate = campaignEndDate;
                    selectedBooking.CampaignLocation = campaignLocation;
                    selectedBooking.RateType = rateType;
                    selectedBooking.Duration = duration;
                    selectedBooking.BookingFee = bookingFee;
                    selectedBooking.ActiveStatus = activeStatus;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE Booking SET " +
                            "SpecialRequirement = @SpecialRequirement, " +
                            "ClientID = @ClientID, " +
                            "CampaignID = @CampaignID, " +
                            "StaffID = @StaffID, " +
                            "BookingTime = @BookingTime, " +
                            "CampaignStartDate = @CampaignStartDate, " +
                            "CampaignEndDate = @CampaignEndDate, " +
                            "CampaignLocation = @CampaignLocation, " +
                            "RateType = @RateType, " +
                            "Duration = @Duration, " +
                            "BookingFee = @BookingFee, " +
                            "ActiveStatus = @ActiveStatus " +
                            "WHERE BookingID = @BookingID";

                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@SpecialRequirement", selectedBooking.SpecialRequirement);
                        command.Parameters.AddWithValue("@ClientID", selectedBooking.ClientID);
                        command.Parameters.AddWithValue("@CampaignID", selectedBooking.CampaignID);
                        command.Parameters.AddWithValue("@StaffID", selectedBooking.StaffID);
                        command.Parameters.AddWithValue("@BookingTime", selectedBooking.BookingTime);
                        command.Parameters.AddWithValue("@CampaignStartDate", selectedBooking.CampaignStartDate);
                        command.Parameters.AddWithValue("@CampaignEndDate", selectedBooking.CampaignEndDate);
                        command.Parameters.AddWithValue("@CampaignLocation", selectedBooking.CampaignLocation);
                        command.Parameters.AddWithValue("@RateType", selectedBooking.RateType);
                        command.Parameters.AddWithValue("@Duration", selectedBooking.Duration);
                        command.Parameters.AddWithValue("@BookingFee", selectedBooking.BookingFee);
                        command.Parameters.AddWithValue("@ActiveStatus", selectedBooking.ActiveStatus);
                        command.Parameters.AddWithValue("@BookingID", selectedBooking.BookingID);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Booking updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadBookings(); // Refresh the list
                            ClearFields(); // Clear the form fields
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please ensure all fields are filled in correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearFields()
        {
            SpecialRequirementTextBox.Clear();
            ClientIDTextBox.Clear();
            CampaignIDTextBox.Clear();
            StaffIDTextBox.Clear();
            BookingTimeDatePicker.SelectedDate = null;
            CampaignStartDateDatePicker.SelectedDate = null;
            CampaignEndDateDatePicker.SelectedDate = null;
            CampaignLocationTextBox.Clear();
            RateTypeTextBox.Clear();
            DurationTextBox.Clear();
            BookingFeeTextBox.Clear();
            ActiveStatusTextBox.Clear();

            // Clear TalentIDsListBox
            TalentIDsListBox.ItemsSource = null;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
