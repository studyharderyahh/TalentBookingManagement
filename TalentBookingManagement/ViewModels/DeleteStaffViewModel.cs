using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class DeleteStaffViewModel : INotifyPropertyChanged
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        private int _staffId;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private int _age;
        private string _gender;
        private string _email;
        private string _city;
        private string _suburb;
        private string _streetAddress;
        private string _postcode;
        private string _activeStatus;
        private string _username;
        private string _password;
        private int _roleId;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int StaffId
        {
            get => _staffId;
            set
            {
                _staffId = value;
                OnPropertyChanged(nameof(StaffId));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public string Suburb
        {
            get => _suburb;
            set
            {
                _suburb = value;
                OnPropertyChanged(nameof(Suburb));
            }
        }

        public string StreetAddress
        {
            get => _streetAddress;
            set
            {
                _streetAddress = value;
                OnPropertyChanged(nameof(StreetAddress));
            }
        }

        public string Postcode
        {
            get => _postcode;
            set
            {
                _postcode = value;
                OnPropertyChanged(nameof(Postcode));
            }
        }

        public string ActiveStatus
        {
            get => _activeStatus;
            set
            {
                _activeStatus = value;
                OnPropertyChanged(nameof(ActiveStatus));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public int RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                OnPropertyChanged(nameof(RoleId));
            }
        }

        public ICommand LoadStaffDetailsCommand { get; }
        public ICommand DeleteStaffCommand { get; }

        /*public DeleteStaffViewModel()
        {
            LoadStaffDetailsCommand = new RelayCommand(ExecuteLoad);
            DeleteStaffCommand = new RelayCommand(ExecuteDelete);
        }*/

        private void ExecuteLoad(object parameter)
        {
            if (parameter is int staffId)
            {
                Staff staff = GetStaffFromDatabase(staffId);
                if (staff != null)
                {
                    StaffId = staff.StaffID;
                    FirstName = staff.FirstName;
                    LastName = staff.LastName;
                    PhoneNumber = staff.PhoneNumber;
                    Age = staff.Age;
                    Gender = staff.Gender;
                    Email = staff.Email;
                    City = staff.City;
                    Suburb = staff.Suburb;
                    StreetAddress = staff.StreetAddress;
                    Postcode = staff.Postcode.ToString();
                    ActiveStatus = staff.ActiveStatus;
                    Username = staff.Username;
                    Password = staff.Password;
                    RoleId = staff.RoleID;
                }
                else
                {
                    MessageBox.Show("Staff not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Staff ID.");
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

        private void ExecuteDelete(object parameter)
        {
            if (parameter is int staffId)
            {
                bool isDeleted = DeleteStaffInDatabase(staffId);

                if (isDeleted)
                {
                    MessageBox.Show("Staff deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to delete staff.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Staff ID.");
            }
        }

        private bool DeleteStaffInDatabase(int staffId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("LogicalDeleteStaff", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StaffID", staffId);

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
