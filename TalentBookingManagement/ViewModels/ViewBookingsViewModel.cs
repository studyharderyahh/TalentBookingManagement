using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TalentBookingManagement.ViewModels
{
    public class ViewBookingsViewModel : BaseViewModel
    {
        private readonly static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public ObservableCollection<Booking> Bookings { get; set; }

        public ViewBookingsViewModel()
        {
            Bookings = new ObservableCollection<Booking>();
            LoadBookings();
        }

        private void LoadBookings()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetBookingsWithTalent", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Dictionary<int, Booking> bookingDict = new Dictionary<int, Booking>();

                    while (reader.Read())
                    {
                        int bookingID = (int)reader["BookingID"];

                        if (!bookingDict.ContainsKey(bookingID))
                        {
                            Booking booking = new Booking
                            {
                                BookingID = bookingID,
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
                                ActiveStatus = reader["ActiveStatus"].ToString(),
                                TalentIDs = new List<int>()
                            };
                            bookingDict[bookingID] = booking;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("TalentID")))
                        {
                            int talentID = (int)reader["TalentID"];
                            bookingDict[bookingID].TalentIDs.Add(talentID);
                        }
                    }

                    foreach (var booking in bookingDict.Values)
                    {
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
    }
}
