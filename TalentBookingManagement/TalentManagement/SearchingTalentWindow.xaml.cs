using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace TalentBookingManagement
{
    public partial class SearchingTalentWindow : Window
    {
        public SearchingTalentWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTalentData();
        }

        private void LoadTalentData()
        {
            string talentID = TalentIDTextBox.Text;
            string dailyRatesMin = DailyRatesMinTextBox.Text;
            string dailyRatesMax = DailyRatesMaxTextBox.Text;
            string hourlyRatesMin = HourlyRatesMinTextBox.Text;
            string hourlyRatesMax = HourlyRatesMaxTextBox.Text;
            string skill = SkillTextBox.Text;

            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetTalentDetailwithFilter", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TalentID", string.IsNullOrEmpty(talentID) ? (object)DBNull.Value : int.Parse(talentID));
                command.Parameters.AddWithValue("@DailyRatesMin", string.IsNullOrEmpty(dailyRatesMin) ? (object)DBNull.Value : decimal.Parse(dailyRatesMin));
                command.Parameters.AddWithValue("@DailyRatesMax", string.IsNullOrEmpty(dailyRatesMax) ? (object)DBNull.Value : decimal.Parse(dailyRatesMax));
                command.Parameters.AddWithValue("@HourlyRatesMin", string.IsNullOrEmpty(hourlyRatesMin) ? (object)DBNull.Value : decimal.Parse(hourlyRatesMin));
                command.Parameters.AddWithValue("@HourlyRatesMax", string.IsNullOrEmpty(hourlyRatesMax) ? (object)DBNull.Value : decimal.Parse(hourlyRatesMax));
                command.Parameters.AddWithValue("@Skill", string.IsNullOrEmpty(skill) ? (object)DBNull.Value : skill);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                TalentDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}
