using System;
using System.Collections.Generic;
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

namespace TalentBookingManagement
{
    /// <summary>
    /// Interaction logic for AddBookingWindow.xaml
    /// </summary>
    public partial class AddBookingWindow : Window
    {
        public AddBookingWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string specialRequirement = SpecialRequirementTextBox.Text;
            int clientId = int.Parse(ClientIDTextBox.Text);
            int campaignId = int.Parse(CampaignIDTextBox.Text);
            int staffId = int.Parse(StaffIDTextBox.Text);
            DateTime bookingTime = BookingTimeDatePicker.SelectedDate.Value;
            DateTime campaignStartDate = CampaignStartDateDatePicker.SelectedDate.Value;
            DateTime campaignEndDate = CampaignEndDateDatePicker.SelectedDate.Value;
            string campaignLocation = CampaignLocationTextBox.Text;
            string activeStatus = ((ComboBoxItem)ActiveStatusComboBox.SelectedItem).Content.ToString();

            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_YYE_ProjectA;User Id=S2401_Elisa;Password=fBit$98969;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Booking (SpecialRequirement, ClientID, CampaignID, StaffID, BookingTime, CampaignStartDate, CampaignEndDate, CampaignLocation, ActiveStatus) " +
                               "VALUES (@SpecialRequirement, @ClientID, @CampaignID, @StaffID, @BookingTime, @CampaignStartDate, @CampaignEndDate, @CampaignLocation, @ActiveStatus)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpecialRequirement", specialRequirement);
                command.Parameters.AddWithValue("@ClientID", clientId);
                command.Parameters.AddWithValue("@CampaignID", campaignId);
                command.Parameters.AddWithValue("@StaffID", staffId);
                command.Parameters.AddWithValue("@BookingTime", bookingTime);
                command.Parameters.AddWithValue("@CampaignStartDate", campaignStartDate);
                command.Parameters.AddWithValue("@CampaignEndDate", campaignEndDate);
                command.Parameters.AddWithValue("@CampaignLocation", campaignLocation);
                command.Parameters.AddWithValue("@ActiveStatus", activeStatus);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Booking added successfully!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
