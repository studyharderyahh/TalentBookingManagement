using System;
using System.Collections.Generic;
using System.Configuration;
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
using TalentBookingManagement.Models;

namespace TalentBookingManagement.BookingManagement
{
    /// <summary>
    /// Interaction logic for SelectTalentWindow.xaml
    /// </summary>
    public partial class SelectTalentWindow : Window
    {
        public List<Talent> SelectedTalents { get; private set; } = new List<Talent>();

        private static string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public SelectTalentWindow()
        {
            InitializeComponent();
        }

        private void SearchTalent_Click(object sender, RoutedEventArgs e)
        {
            // Get filter values from UI controls
            string availabilityStatus = (cmbAvailabilityStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            string skillName = txtSkillName.Text.Trim();
            DateTime? bookingStartDate = dpStartDate.SelectedDate;
            DateTime? bookingEndDate = dpEndDate.SelectedDate;
            string preferredEngagement = txtPreferredEngagement.Text.Trim();

            decimal? minHourlyRate = null;
            if (decimal.TryParse(txtMinHourlyRate.Text.Trim(), out decimal minHourlyRateValue))
                minHourlyRate = minHourlyRateValue;

            decimal? maxHourlyRate = null;
            if (decimal.TryParse(txtMaxHourlyRate.Text.Trim(), out decimal maxHourlyRateValue))
                maxHourlyRate = maxHourlyRateValue;

            decimal? minDailyRate = null;
            if (decimal.TryParse(txtMinDailyRate.Text.Trim(), out decimal minDailyRateValue))
                minDailyRate = minDailyRateValue;

            decimal? maxDailyRate = null;
            if (decimal.TryParse(txtMaxDailyRate.Text.Trim(), out decimal maxDailyRateValue))
                maxDailyRate = maxDailyRateValue;

            // Call stored procedure to fetch filtered talents
            List<Talent> filteredTalents = FetchFilteredTalents(availabilityStatus, skillName, bookingStartDate, bookingEndDate, preferredEngagement, minHourlyRate, maxHourlyRate, minDailyRate, maxDailyRate);

            // Bind fetched talents to the ListBox
            lstTalents.ItemsSource = filteredTalents;
        }

        private List<Talent> FetchFilteredTalents(string availabilityStatus, string skillName, DateTime? bookingStartDate, DateTime? bookingEndDate,
                                                  string preferredEngagement, decimal? minHourlyRate, decimal? maxHourlyRate,
                                                  decimal? minDailyRate, decimal? maxDailyRate)
        {
            List<Talent> talents = new List<Talent>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spGetFilteredTalent", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters and handle nulls correctly
                    cmd.Parameters.AddWithValue("@AvailabilityStatus", string.IsNullOrEmpty(availabilityStatus) ? (object)DBNull.Value : availabilityStatus);
                    cmd.Parameters.AddWithValue("@SkillName", string.IsNullOrEmpty(skillName) ? (object)DBNull.Value : skillName);
                    cmd.Parameters.AddWithValue("@BookingStartDate", bookingStartDate.HasValue ? (object)bookingStartDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@BookingEndDate", bookingEndDate.HasValue ? (object)bookingEndDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PreferredEngagement", string.IsNullOrEmpty(preferredEngagement) ? (object)DBNull.Value : preferredEngagement);
                    cmd.Parameters.AddWithValue("@MinHourlyRate", minHourlyRate.HasValue ? (object)minHourlyRate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaxHourlyRate", maxHourlyRate.HasValue ? (object)maxHourlyRate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@MinDailyRate", minDailyRate.HasValue ? (object)minDailyRate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaxDailyRate", maxDailyRate.HasValue ? (object)maxDailyRate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ActiveStatus", "Active"); // Assuming default active status

                    connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Talent talent = new Talent
                        {
                            TalentID = rdr["TalentID"] != DBNull.Value ? Convert.ToInt32(rdr["TalentID"]) : 0,
                            FirstName = rdr["FirstName"] != DBNull.Value ? rdr["FirstName"].ToString() : string.Empty,
                            LastName = rdr["LastName"] != DBNull.Value ? rdr["LastName"].ToString() : string.Empty,
                            PhoneNumber = rdr["PhoneNumber"] != DBNull.Value ? rdr["PhoneNumber"].ToString() : string.Empty,
                            Email = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString() : string.Empty,
                            City = rdr["City"] != DBNull.Value ? rdr["City"].ToString() : string.Empty,
                            Suburb = rdr["Suburb"] != DBNull.Value ? rdr["Suburb"].ToString() : string.Empty,
                            StreetAddress = rdr["StreetAddress"] != DBNull.Value ? rdr["StreetAddress"].ToString() : string.Empty,
                            Postcode = rdr["Postcode"] != DBNull.Value ? rdr["Postcode"].ToString() : string.Empty,
                            Skill = rdr["Skill"] != DBNull.Value ? rdr["Skill"].ToString() : string.Empty,
                            BookingID = rdr["BookingID"] != DBNull.Value ? Convert.ToInt32(rdr["BookingID"]) : 0,
                            BookingStartDate = rdr["BookingStartDate"] != DBNull.Value ? (DateTime)rdr["BookingStartDate"] : default(DateTime),
                            BookingEndDate = rdr["BookingEndDate"] != DBNull.Value ? (DateTime)rdr["BookingEndDate"] : default(DateTime),
                            SpecialRequirement = rdr["SpecialRequirement"] != DBNull.Value ? rdr["SpecialRequirement"].ToString() : string.Empty,
                            CampaignLocation = rdr["CampaignLocation"] != DBNull.Value ? rdr["CampaignLocation"].ToString() : string.Empty,
                            PreferredEngagement = rdr["PreferredEngagement"] != DBNull.Value ? rdr["PreferredEngagement"].ToString() : string.Empty,
                            HourlyRates = rdr["HourlyRates"] != DBNull.Value ? Convert.ToDecimal(rdr["HourlyRates"]) : 0,
                            DailyRates = rdr["DailyRates"] != DBNull.Value ? Convert.ToDecimal(rdr["DailyRates"]) : 0
                        };

                        talents.Add(talent);
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching talents: " + ex.Message);
            }

            return talents;
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            //SelectedTalents.Clear();
            foreach (Talent talent in lstTalents.SelectedItems)
            {
                SelectedTalents.Add(talent);
            }

            DialogResult = true;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public List<int> SelectedTalentIDs
        {
            get
            {
                List<int> selectedTalentIDs = new List<int>();
                foreach (Talent talent in lstTalents.SelectedItems)
                {
                    selectedTalentIDs.Add(talent.TalentID);
                }
                return selectedTalentIDs;
            }
        }
    }
}
