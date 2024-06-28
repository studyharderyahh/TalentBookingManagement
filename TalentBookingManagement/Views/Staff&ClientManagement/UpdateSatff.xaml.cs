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

namespace TalentBookingManagement.Views.Staff_ClientManagement
{
    public partial class UpdateSatff : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;
        public UpdateSatff()
        {
            InitializeComponent();
        }
        private void LoadStaffDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(StaffIDInputTextBox.Text, out int staffId))
            {
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(StaffIDTextBox.Text, out int staffId))
            {
                Staff staff = new Staff
                {
                    StaffID = staffId,
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    PhoneNumber = PhoneNumberTextBox.Text,
                    Age = int.Parse(AgeTextBox.Text),
                    Gender = GenderTextBox.Text,
                    Email = EmailTextBox.Text,
                    City = CityTextBox.Text,
                    Suburb = SuburbTextBox.Text,
                    StreetAddress = StreetAddressTextBox.Text,
                    Postcode = PostcodeTextBox.Text,
                    ActiveStatus = ActiveStatusTextBox.Text,
                    Username = UsernameTextBox.Text,
                    Password = PasswordTextBox.Text,
                    RoleID = int.Parse(RoleIDTextBox.Text)
                };

                if (staff.ActiveStatus == "Active")
                {
                    bool isUpdated = UpdateStaffInDatabase(staff);

                    if (isUpdated)
                    {
                        MessageBox.Show("Staff details updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update staff details.");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot update inactive staff details.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Staff ID.");
            }
        }

        private bool UpdateStaffInDatabase(Staff staff)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("UpdateStaff", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StaffID", staff.StaffID);
                        command.Parameters.AddWithValue("@FirstName", staff.FirstName);
                        command.Parameters.AddWithValue("@LastName", staff.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", staff.PhoneNumber);
                        command.Parameters.AddWithValue("@Age", staff.Age);
                        command.Parameters.AddWithValue("@Gender", staff.Gender);
                        command.Parameters.AddWithValue("@Email", staff.Email);
                        command.Parameters.AddWithValue("@City", staff.City);
                        command.Parameters.AddWithValue("@Suburb", staff.Suburb);
                        command.Parameters.AddWithValue("@StreetAddress", staff.StreetAddress);
                        command.Parameters.AddWithValue("@Postcode", staff.Postcode);
                        command.Parameters.AddWithValue("@ActiveStatus", staff.ActiveStatus);
                        command.Parameters.AddWithValue("@Username", staff.Username);
                        command.Parameters.AddWithValue("@Password", staff.Password);
                        command.Parameters.AddWithValue("@RoleID", staff.RoleID);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }
    }
}
