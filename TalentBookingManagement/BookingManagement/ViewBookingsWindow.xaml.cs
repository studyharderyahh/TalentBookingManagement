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
    /// Interaction logic for ViewBookingsWindow.xaml
    /// </summary>
    public partial class ViewBookingsWindow : Window
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public ObservableCollection<Booking> Bookings { get; set; }

        public ViewBookingsWindow()
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
                            BookingTime = (DateTime)reader["BookingTime"],
                            CampaignStartDate = (DateTime)reader["CampaignStartDate"],
                            CampaignEndDate = (DateTime)reader["CampaignEndDate"],
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
    }
}
