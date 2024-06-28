using System;
using System.Collections.Generic;
using System.Data;
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
using TalentBookingManagement.Models;
using TalentBookingManagement.DatabaseManagement;



namespace TalentBookingManagement.Staff_ClientManagement
{
    public partial class AddNewStaff : Window
    {
        public AddNewStaff()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddNewStaff", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                command.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text);
                command.Parameters.AddWithValue("@Age", int.Parse(AgeTextBox.Text));
                command.Parameters.AddWithValue("@Gender", (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                command.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                command.Parameters.AddWithValue("@City", CityTextBox.Text);
                command.Parameters.AddWithValue("@Suburb", SuburbTextBox.Text);
                command.Parameters.AddWithValue("@StreetAddress", StreetAddressTextBox.Text);
                command.Parameters.AddWithValue("@Postcode", PostcodeTextBox.Text);
                command.Parameters.AddWithValue("@ActiveStatus", (ActiveStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                command.Parameters.AddWithValue("@Username", UsernameTextBox.Text);
                command.Parameters.AddWithValue("@Password", PasswordBox.Password);
                command.Parameters.AddWithValue("@RoleID", int.Parse(RoleIDTextBox.Text));

                // Add an output parameter for returning the StaffID
                SqlParameter outputParameter = new SqlParameter();
                outputParameter.ParameterName = "@StaffID";
                outputParameter.SqlDbType = SqlDbType.Int;
                outputParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputParameter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    int newStaffId = (int)command.Parameters["@StaffID"].Value;

                    MessageBox.Show($"New staff added with ID: {newStaffId}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
