using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace TalentBookingManagement
{
    public partial class ReadTalentListWindow : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public ReadTalentListWindow()
        {
            InitializeComponent();
        }

        private void LoadTalentListButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTalentList();
        }

        private void LoadTalentList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetTalentDetails", connection);
                command.CommandType = CommandType.StoredProcedure;


                int talentID;
                if (int.TryParse(TalentIDTextBox.Text, out talentID))
                {
                    command.Parameters.AddWithValue("@TalentID", talentID);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    TalentDataGrid.ItemsSource = dataTable.DefaultView;
                }
                else
                {
                    MessageBox.Show("Please enter a valid Talent ID.");
                }
            }
        }
    }
}