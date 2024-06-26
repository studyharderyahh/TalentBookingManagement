using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TalentBookingManagement
{
    public partial class AddTalentWindow : Window
    {
        public AddTalentWindow()
        {
            InitializeComponent();
        }

        private void AddTalentButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=citizen.manukautech.info,6306;Database=S601_LetItGo_Project;User Id=S601_LetItGo;Password=fBit$26170;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddTalent", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
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
                command.Parameters.AddWithValue("@AvailabilityStatus", (AvailabilityStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                command.Parameters.AddWithValue("@ActiveStatus", (ActiveStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                command.Parameters.AddWithValue("@SkillNames", SkillNamesTextBox.Text);

                SqlParameter talentIdParam = new SqlParameter("@TalentID", SqlDbType.Int);
                talentIdParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(talentIdParam);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    int newTalentId = (int)command.Parameters["@TalentID"].Value;

                    MessageBox.Show($"New talent added with ID: {newTalentId}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                textBox.Text = textBox.Name.Replace("TextBox", "").Replace("ID", " ID").Replace("PhoneNumber", "Phone Number").Replace("Email", "Email").Replace("Age", "Age").Replace("Gender", "Gender").Replace("City", "City").Replace("Suburb", "Suburb").Replace("StreetAddress", "Street Address").Replace("Postcode", "Postcode").Replace("AvailabilityStatus", "Availability Status").Replace("ActiveStatus", "Active Status").Replace("SkillNames", "Skills (comma-separated)");
            }
        }

        private void CityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
