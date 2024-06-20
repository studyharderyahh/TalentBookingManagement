using System;
using System.Collections.Generic;
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
using TalentBookingManagement.BookingManagement;
using TalentBookingManagement.Models;
using TalentBookingManagement.TalentManagement;
using System.Configuration;

namespace TalentBookingManagement.BookingManagement
{
    /// <summary>
    /// Interaction logic for MakeBookingWindow.xaml
    /// </summary>
    public partial class MakeBookingWindow : Window
    {
        private List<int> selectedTalentIDs = new List<int>();
        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        public MakeBookingWindow()
        {
            InitializeComponent();
        }

        private void CheckClient_Click(object sender, RoutedEventArgs e)
        {
            string clientInput = txtClientInput.Text.Trim();
            string selectedOption = ((ComboBoxItem)cmbClientOption.SelectedItem).Content.ToString();

            if (selectedOption == "ClientPhoneNumber")
            {
                GetClientByPhoneNumber(clientInput);
            }
            else if (selectedOption == "ClientID")
            {
                int clientId;
                if (int.TryParse(clientInput, out clientId))
                {
                    GetClientByID(clientId);
                }
                else
                {
                    MessageBox.Show("Invalid ClientID. Please enter a valid ClientID.");
                }
            }
        }

        private void GetClientByPhoneNumber(string phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetClientByPhoneNumber", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int clientId = Convert.ToInt32(reader["PersonID"]);
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string retrievedPhoneNumber = reader["PhoneNumber"].ToString();

                            DisplayClientDetails(clientId, firstName, lastName, retrievedPhoneNumber);
                            clientDetailsBorder.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client not found. Please create a new client.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving client details: " + ex.Message);
                }
            }
        }

        private void GetClientByID(int clientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("spGetClientInfo", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ClientID", clientId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int clientID = Convert.ToInt32(reader["ClientID"]);
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string phoneNumber = reader["PhoneNumber"].ToString();

                            DisplayClientDetails(clientID, firstName, lastName, phoneNumber);
                            clientDetailsBorder.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client not found. Please create a new client.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving client details: " + ex.Message);
                }
            }
        }

        private void DisplayClientDetails(int clientID, string firstName, string lastName, string phoneNumber)
        {
            txtClientID.Text = clientID.ToString();
            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;
            txtPhoneNumber.Text = phoneNumber;
        }

        private void ClearClientDetails()
        {
            // Clear client details from UI
            txtClientID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;

            // Hide client details border
            clientDetailsBorder.Visibility = Visibility.Collapsed;
        }

        private void cmbCampaignDetail_DropDownOpened(object sender, EventArgs e)
        {
            LoadCampaigns(); // Load campaigns when ComboBox dropdown is opened
        }

        private void LoadCampaigns()
        {
            cmbCampaignDetail.Items.Clear(); // Clear existing items

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetAllCampaigns", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Assuming CampaignName is a column in your result set
                        string campaignName = reader["CampaignName"].ToString();
                        cmbCampaignDetail.Items.Add(campaignName);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading campaigns: " + ex.Message);
            }
        }
        private void cmbCampaignDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCampaignDetail.SelectedItem != null)
            {
                string selectedCampaign = cmbCampaignDetail.SelectedItem.ToString();
                cmbCampaignDetail.Text = selectedCampaign; // Display selected campaign name in ComboBox text
            }
        }

        private void CreateClient_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to create a new client
            var createClientWindow = new CreateClientWindow();
            if (createClientWindow.ShowDialog() == true)
            {
                txtClientID.Text = createClientWindow.CreatedClient.ClientID.ToString();
                MessageBox.Show("Client created successfully.", "Create Client");
            }
        }

        private void CreateCampaign_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to create a new campaign
            var createCampaignWindow = new CreateCampaignWindow();
            if (createCampaignWindow.ShowDialog() == true)
            {
                cmbCampaignDetail.ItemsSource = GetAllCampaigns();
                cmbCampaignDetail.SelectedItem = createCampaignWindow.CreatedCampaign;
                MessageBox.Show("Campaign created successfully.", "Create Campaign");
            }
        }

        private void RateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRateType.SelectedItem != null)
            {
                var selectedRateType = (cmbRateType.SelectedItem as ComboBoxItem).Content.ToString();
                if (selectedRateType == "Hourly" || selectedRateType == "Daily")
                {
                    lblDuration.Visibility = Visibility.Visible;
                    txtDuration.Visibility = Visibility.Visible;
                    lblDuration.Content = selectedRateType == "Hourly" ? "Hours:" : "Days:";
                }
                CalculateBookingFee();
            }
        }

        private void SelectTalent_Click(object sender, RoutedEventArgs e)
        {
            var selectTalentWindow = new SelectTalentWindow();
            if (selectTalentWindow.ShowDialog() == true)
            {
                selectedTalentIDs = selectTalentWindow.SelectedTalentIDs;
                lstSelectedTalents.ItemsSource = selectTalentWindow.SelectedTalents;
                CalculateBookingFee();
            }
        }

        private void CalculateBookingFee()
        {
            if (cmbCampaignDetail.SelectedItem == null || cmbRateType.SelectedItem == null || string.IsNullOrEmpty(txtDuration.Text))
            {
                txtBookingFee.Text = "0";
                return;
            }

            var selectedCampaign = cmbCampaignDetail.SelectedItem as Campaign;
            var selectedRateType = (cmbRateType.SelectedItem as ComboBoxItem).Content.ToString();
            int duration;
            if (!int.TryParse(txtDuration.Text, out duration))
            {
                txtBookingFee.Text = "0";
                return;
            }

            decimal rate = selectedRateType == "Hourly" ? selectedCampaign.HourlyRate : selectedCampaign.DailyRate;
            decimal totalFee = rate * duration * selectedTalentIDs.Count;

            txtBookingFee.Text = totalFee.ToString("C");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate fields and create booking
            if (ValidateBookingDetails())
            {
                Booking newBooking = new Booking
                {
                    StaffID = int.Parse(txtStaffID.Text),
                    ClientID = int.Parse(txtClientID.Text),
                    CampaignID = (cmbCampaignDetail.SelectedItem as Campaign).CampaignID,
                    CampaignStartDate = dpCampaignStartDate.SelectedDate.Value,
                    CampaignEndDate = dpCampaignEndDate.SelectedDate.Value,
                    CampaignLocation = txtCampaignLocation.Text,
                    SpecialRequirement = txtSpecialRequirement.Text,
                    RateType = (cmbRateType.SelectedItem as ComboBoxItem).Content.ToString(),
                    Duration = int.Parse(txtDuration.Text),
                    BookingFee = decimal.Parse(txtBookingFee.Text, System.Globalization.NumberStyles.Currency),
                    TalentIDs = selectedTalentIDs
                };

                AddBooking(newBooking);
                MessageBox.Show("Booking created successfully.", "Create Booking");
                this.Close();
            }
        }

        private bool ValidateBookingDetails()
        {
            // Implement validation logic for booking details
            if (string.IsNullOrEmpty(txtClientID.Text) || cmbCampaignDetail.SelectedItem == null ||
                !dpCampaignStartDate.SelectedDate.HasValue ||
                !dpCampaignEndDate.SelectedDate.HasValue || string.IsNullOrEmpty(txtCampaignLocation.Text) ||
                cmbRateType.SelectedItem == null || string.IsNullOrEmpty(txtDuration.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error");
                return false;
            }

            if (dpCampaignStartDate.SelectedDate.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Campaign Start Date cannot be in the past.", "Validation Error");
                return false;
            }

            if (dpCampaignEndDate.SelectedDate.Value < dpCampaignStartDate.SelectedDate.Value)
            {
                MessageBox.Show("Campaign End Date cannot be earlier than Campaign Start Date.", "Validation Error");
                return false;
            }

            return true;
        }

        public static List<Campaign> GetAllCampaigns()
        {
            List<Campaign> campaigns = new List<Campaign>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetAllCampaigns", connection);
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campaign campaign = new Campaign
                        {
                            CampaignID = Convert.ToInt32(reader["CampaignID"]),
                            CampaignType = reader["CampaignName"].ToString(),
                            HourlyRate = Convert.ToDecimal(reader["HourlyRates"]),
                            DailyRate = Convert.ToDecimal(reader["DailyRates"])
                        };

                        campaigns.Add(campaign);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle exception (log or throw)
                    Console.WriteLine(ex.Message);
                }
            }

            return campaigns;
        }
        public void AddBooking(Booking newBooking)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddBooking", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ClientID", newBooking.ClientID);
                command.Parameters.AddWithValue("@CampaignID", newBooking.CampaignID);
                command.Parameters.AddWithValue("@StaffID", newBooking.StaffID);
                command.Parameters.AddWithValue("@BookingTime", newBooking.BookingTime);
                command.Parameters.AddWithValue("@CampaignStartDate", newBooking.CampaignStartDate);
                command.Parameters.AddWithValue("@CampaignEndDate", newBooking.CampaignEndDate);
                command.Parameters.AddWithValue("@CampaignLocation", newBooking.CampaignLocation);
                command.Parameters.AddWithValue("@SpecialRequirement", newBooking.SpecialRequirement);
                command.Parameters.AddWithValue("@RateType", newBooking.RateType);
                command.Parameters.AddWithValue("@Duration", newBooking.Duration);
                command.Parameters.AddWithValue("@BookingFee", newBooking.BookingFee);

                // Create table-valued parameter for talent IDs
                DataTable talentIDsTable = new DataTable();
                talentIDsTable.Columns.Add("TalentID", typeof(int));
                foreach (var talentID in newBooking.TalentIDs)
                {
                    talentIDsTable.Rows.Add(talentID);
                }
                SqlParameter talentIDsParam = command.Parameters.AddWithValue("@TalentIDs", talentIDsTable);
                talentIDsParam.SqlDbType = SqlDbType.Structured;
                talentIDsParam.TypeName = "dbo.IntList";

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Booking created successfully.", "Create Booking");
                }
                catch (Exception ex)
                {
                    // Handle exception (log or throw)
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error");
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
