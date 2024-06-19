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
    /// Interaction logic for ViewBookingWindow.xaml
    /// </summary>
    public partial class ViewBookingsWindow : Window
    {
        public ObservableCollection<Booking> Bookings { get; set; }

        public ViewBookingsWindow()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings()
        {
            Bookings = new ObservableCollection<Booking>();

            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_YYE_ProjectA;User Id=S2401_Elisa;Password=fBit$98969;";

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

            BookingsDataGrid.ItemsSource = Bookings;
        }
    }
}
