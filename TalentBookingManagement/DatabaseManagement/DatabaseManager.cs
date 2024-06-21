using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentBookingManagement.Models;
using System.Configuration;

namespace TalentBookingManagement.DatabaseHelper
{
    public class DatabaseManager
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

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
                            CampaignName = reader["CampaignName"].ToString(),
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

        // Add other methods for managing clients, bookings, etc.
    }
}
