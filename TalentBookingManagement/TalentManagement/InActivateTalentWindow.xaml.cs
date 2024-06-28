using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace TalentBookingManagement
{
    public partial class InactiveTalentWindow : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public InactiveTalentWindow()
        {
            InitializeComponent();
        }

        // Search button click event handler
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int talentID;
            // Check if Talent ID is a valid integer
            if (int.TryParse(TalentIDTextBox.Text, out talentID))
            {
                LoadTalentData(talentID);
            }
            else
            {
                MessageBox.Show("Please enter a valid Talent ID.");
            }
        }

        // Inactivate button click event handler
        private void InactivateButton_Click(object sender, RoutedEventArgs e)
        {
            int talentID;
            // Check if Talent ID is a valid integer
            if (int.TryParse(TalentIDTextBox.Text, out talentID))
            {
                // Display confirmation message
                MessageBoxResult result = MessageBox.Show($"Talent ID: {talentID}\nDo you want to proceed inactivating this talent?", "Confirm Inactivation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    InactivateTalent(talentID);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Talent ID.");
            }
        }

        // Method to load talent data
        private void LoadTalentData(int talentID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL command to retrieve talent data
                SqlCommand command = new SqlCommand("SELECT * FROM Person INNER JOIN Talent ON Person.PersonID = Talent.PersonID WHERE Talent.TalentID = @TalentID", connection);
                command.Parameters.AddWithValue("@TalentID", talentID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Display retrieved data in the data grid
                TalentDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        // Method to inactivate talent
        private void InactivateTalent(int talentID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Stored procedure to inactivate talent
                SqlCommand command = new SqlCommand("DeleteTalent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TalentID", talentID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Talent inactivated successfully.");
                    LoadTalentData(talentID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}