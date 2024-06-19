using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace TalentBookingManagement
{
    public partial class InactiveTalentWindow : Window
    {
        public InactiveTalentWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int talentID;
            if (int.TryParse(TalentIDTextBox.Text, out talentID))
            {
                LoadTalentData(talentID);
            }
            else
            {
                MessageBox.Show("Please enter a valid Talent ID.");
            }
        }

        private void InactivateButton_Click(object sender, RoutedEventArgs e)
        {
            int talentID;
            if (int.TryParse(TalentIDTextBox.Text, out talentID))
            {
                InactivateTalent(talentID);
            }
            else
            {
                MessageBox.Show("Please enter a valid Talent ID.");
            }
        }

        private void LoadTalentData(int talentID)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Person INNER JOIN Talent ON Person.PersonID = Talent.PersonID WHERE Talent.TalentID = @TalentID", connection);
                command.Parameters.AddWithValue("@TalentID", talentID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                TalentDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void InactivateTalent(int talentID)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
