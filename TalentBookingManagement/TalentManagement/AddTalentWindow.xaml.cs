using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TalentBookingManagement
{
    public partial class AddTalentWindow : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public AddTalentWindow()
        {
            InitializeComponent();
        }

        // Add Talent button click event handler
        private void AddTalentButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (IsValid())
            {
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

                    // Output parameter to get the new Talent ID
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
        }

        // Validation check for input fields
        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("First Name is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Last Name is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Phone Number is required.");
                return false;
            }
            // Check if Age is null or not a valid integer
            if (string.IsNullOrWhiteSpace(AgeTextBox.Text) || !int.TryParse(AgeTextBox.Text, out _))
            {
                MessageBox.Show("Valid Age is required.");
                return false;
            }
            if (GenderComboBox.SelectedItem == null)
            {
                MessageBox.Show("Gender is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Email is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                MessageBox.Show("City is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(SuburbTextBox.Text))
            {
                MessageBox.Show("Suburb is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(StreetAddressTextBox.Text))
            {
                MessageBox.Show("Street Address is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PostcodeTextBox.Text))
            {
                MessageBox.Show("Postcode is required.");
                return false;
            }
            if (AvailabilityStatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Availability Status is required.");
                return false;
            }
            if (ActiveStatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Active Status is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(SkillNamesTextBox.Text))
            {
                MessageBox.Show("Skill Names are required.");
                return false;
            }
            return true;
        }

        // Remove placeholder text when the TextBox gets focus
        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        // Add placeholder text when the TextBox loses focus and is empty
        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                textBox.Text = textBox.Name.Replace("TextBox", "").Replace("ID", " ID").Replace("PhoneNumber", "Phone Number").Replace("Email", "Email").Replace("Age", "Age").Replace("Gender", "Gender").Replace("City", "City").Replace("Suburb", "Suburb").Replace("StreetAddress", "Street Address").Replace("Postcode", "Postcode").Replace("AvailabilityStatus", "Availability Status").Replace("ActiveStatus", "Active Status").Replace("SkillNames", "Skills (comma-separated)");
            }
        }

        // Event handler for City TextBox text changed event (empty in this example)
        private void CityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}