using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using TalentBookingManagement.Models;
using System.Configuration;

namespace TalentBookingManagement.Staff_ClientManagement
{
    /// <summary>
    /// Interaction logic for ViewStaffDetails.xaml
    /// </summary>
    public partial class ViewStaffDetails : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public ViewStaffDetails()
        {
            InitializeComponent();
        }

        private void LoadStaffDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the Staff ID entered by the user
            if (int.TryParse(StaffIDInputTextBox.Text, out int staffId))
            {
                // Load and display staff details based on the entered Staff ID
                LoadStaffDetails(staffId);
            }
            else
            {
                MessageBox.Show("Please enter a valid Staff ID.");
            }
        }

        private void LoadStaffDetails(int staffId)
        {

            Staff staff = GetStaffFromDatabase(staffId);

            if (staff != null)
            {
                StaffIDTextBox.Text = staff.StaffID.ToString();
                FirstNameTextBox.Text = staff.FirstName;
                LastNameTextBox.Text = staff.LastName;
                PhoneNumberTextBox.Text = staff.PhoneNumber;
                AgeTextBox.Text = staff.Age.ToString();
                GenderTextBox.Text = staff.Gender;
                EmailTextBox.Text = staff.Email;
                CityTextBox.Text = staff.City;
                SuburbTextBox.Text = staff.Suburb;
                StreetAddressTextBox.Text = staff.StreetAddress;
                PostcodeTextBox.Text = staff.Postcode.ToString();
                ActiveStatusTextBox.Text = staff.ActiveStatus;
                UsernameTextBox.Text = staff.Username;
                PasswordTextBox.Text = staff.Password;
                RoleIDTextBox.Text = staff.RoleID.ToString();

                StaffDetailsStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Staff not found.");
            }
        }

        private Staff GetStaffFromDatabase(int staffId)
        {
           Staff staff = null;
            /*string query = "SELECT s.StaffID, p.FirstName, p.LastName, p.PhoneNumber, p.Age, p.Gender, p.Email, p.City, p.Suburb, p.StreetAddress, p.Postcode, p.ActiveStatus, s.Username, s.Password, s.RoleID " +
                           "FROM Staff s " +
                           "JOIN Person p ON s.PersonID = p.PersonID " +
                           "WHERE s.StaffID = @StaffID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StaffID", staffId); // Assuming staffId is the ID you want to filter on
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Staff staff = new Staff
                    {
                        StaffID = Convert.ToInt32(reader["StaffID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Gender = reader["Gender"].ToString(),
                        Email = reader["Email"].ToString(),
                        City = reader["City"].ToString(),
                        Suburb = reader["Suburb"].ToString(),
                        StreetAddress = reader["StreetAddress"].ToString(),
                        Postcode = reader["Postcode"].ToString(),
                        ActiveStatus = reader["ActiveStatus"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        RoleID = Convert.ToInt32(reader["RoleID"])
                    };*/
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("ViewStaffDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StaffID", staffId);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                staff = new Staff
                                {
                                    StaffID = Convert.ToInt32(reader["StaffID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Age = Convert.ToInt32(reader["Age"]),
                                    Gender = reader["Gender"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    City = reader["City"].ToString(),
                                    Suburb = reader["Suburb"].ToString(),
                                    StreetAddress = reader["StreetAddress"].ToString(),
                                    Postcode = reader["Postcode"].ToString(),
                                    ActiveStatus = reader["ActiveStatus"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    RoleID = Convert.ToInt32(reader["RoleID"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }


            return staff;
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}


