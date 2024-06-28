using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Configuration;
using TalentBookingManagement.Models;
using System.Windows.Controls;

namespace TalentBookingManagement.ViewModels
{
    public class MakeBookingViewModel : BaseViewModel
    { 
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        private int _bookingId;
        private ObservableCollection<Campaign> campaigns;
        private Campaign _selectedCampaign;
        private ObservableCollection<Talent> _selectedTalents;
        private string _bookingFee;
        private string _clientInput;
        private int _selectedClientOption;
        private bool _isClientDetailsVisible;
        private string _clientId;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _staffID;
        private DateTime? campaignstartDate;
        private DateTime? _campaignEndDate;
        private string _campaignLocation;
        private string _specialRequirement;
        private string _rateType;
        private bool _isDurationVisible;
        private string _duration;
        private string _durationLabel;


        public MakeBookingViewModel()
        {
            CheckClientCommand = new RelayCommand(CheckClient);
            CreateClientCommand = new RelayCommand(CreateClient);
            CreateCampaignCommand = new RelayCommand(CreateCampaign);
            SelectTalentCommand = new RelayCommand(SelectTalent);
            CalculateBookingFeeCommand = new RelayCommand(CalculateBookingFee);
            CreateBookingCommand = new RelayCommand(CreateBooking);
            LoadCampaignsCommand = new RelayCommand(LoadCampaigns);
            Campaigns = new ObservableCollection<Campaign>();
            SelectedTalents = new ObservableCollection<Talent>();
            RateTypes = new ObservableCollection<string> { "Hourly", "Daily" };
            LoadCampaigns();
        }

        public ICommand CheckClientCommand { get; }
        public ICommand CreateClientCommand { get; }
        public ICommand LoadCampaignsCommand { get; set; }
        public ICommand CreateCampaignCommand { get; }
        public ICommand SelectTalentCommand { get; }
        public ICommand CalculateBookingFeeCommand { get; }
        public ICommand CreateBookingCommand { get; }
        public ICommand CloseBookingCommand { get; }
        public ObservableCollection<string> RateTypes { get; }

        public string ClientInput
        {
            get => _clientInput;
            set => SetProperty(ref _clientInput, value);
        }

        public int SelectedClientOption
        {
            get => _selectedClientOption;
            set => SetProperty(ref _selectedClientOption, value);
        }

        public bool IsClientDetailsVisible
        {
            get => _isClientDetailsVisible;
            set => SetProperty(ref _isClientDetailsVisible, value);
        }

        public string ClientId
        {
            get => _clientId;
            set => SetProperty(ref _clientId, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public ObservableCollection<Campaign> Campaigns
        {
            get => campaigns;
            set => SetProperty(ref campaigns, value);
        }

        public Campaign SelectedCampaign
        {
            get => _selectedCampaign;
            set => SetProperty(ref _selectedCampaign, value);
        }

        public string StaffID
        {
            get => _staffID;
            set => SetProperty(ref _staffID, value);
        }

        public DateTime? CampaignStartDate
        {
            get => campaignstartDate;
            set => SetProperty(ref campaignstartDate, value);
        }

        public DateTime? CampaignEndDate
        {
            get => _campaignEndDate;
            set => SetProperty(ref _campaignEndDate, value);
        }

        public string CampaignLocation
        {
            get => _campaignLocation;
            set => SetProperty(ref _campaignLocation, value);
        }

        public string SpecialRequirement
        {
            get => _specialRequirement;
            set => SetProperty(ref _specialRequirement, value);
        }

        public string RateType
        {
            get => _rateType;
            set
            {
                if (SetProperty(ref _rateType, value))
                {
                    RateType_SelectionChanged(_rateType);
                }
            }
        }

        public bool IsDurationVisible
        {
            get => _isDurationVisible;
            set => SetProperty(ref _isDurationVisible, value);
        }

        public string Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public string DurationLabel
        {
            get => _durationLabel;
            set => SetProperty(ref _durationLabel, value);
        }

        public ObservableCollection<Talent> SelectedTalents
        {
            get => _selectedTalents;
            set => SetProperty(ref _selectedTalents, value);
        }

        public string BookingFee
        {
            get => _bookingFee;
            set => SetProperty(ref _bookingFee, value);
        }
        public int BookingID
        {
            get => _bookingId;
            set => SetProperty(ref _bookingId, value);
        }

        private void CheckClient()
        {
            if (SelectedClientOption == 0)
            {
                GetClientByPhoneNumber(ClientInput);
            }
            else if (SelectedClientOption == 1)
            {
                if (int.TryParse(ClientInput, out int clientId))
                {
                    GetClientByID(clientId);
                }
                else
                {
                    MessageBox.Show("Invalid ClientID. Please enter a valid ClientID.");
                }
            }
        }

        private void GetClientByPhoneNumber(string phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetClientByPhoneNumber", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int clientID = Convert.ToInt32(reader["ClientID"]);
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string retrievedPhoneNumber = reader["PhoneNumber"].ToString();

                            DisplayClientDetails(clientID, firstName, lastName, retrievedPhoneNumber);
                            IsClientDetailsVisible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client not found. Please create a new client.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving client details: " + ex.Message);
                }
            }
        }

        private void GetClientByID(int clientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("spGetClientInfo", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@ClientID", clientId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int retrievedClientID = Convert.ToInt32(reader["ClientID"]);
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string phoneNumber = reader["PhoneNumber"].ToString();

                            DisplayClientDetails(retrievedClientID, firstName, lastName, phoneNumber);
                            IsClientDetailsVisible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client not found. Please create a new client.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving client details: " + ex.Message);
                }
            }
        }

        private void DisplayClientDetails(int clientID, string firstName, string lastName, string phoneNumber)
        {
            ClientId = clientID.ToString();
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        private void CreateClient()
        {
            // Implement logic to create a new client
            var createClientWindow = new CreateClientWindow();
            if (createClientWindow.ShowDialog() == true)
            {
                ClientId = createClientWindow.CreatedClient.ClientID.ToString();
                MessageBox.Show($"Client created successfully, new ClientID is:{ClientId}.", "Create Client");
            }
        }

        private void CreateCampaign()
        {
            // Implement logic to create a new campaign
            var createCampaignWindow = new CreateCampaignWindow();
            if (createCampaignWindow.ShowDialog() == true)
            {
                if (createCampaignWindow.CreatedCampaign != null)
                {
                    Campaigns.Add(createCampaignWindow.CreatedCampaign);
                    SelectedCampaign = createCampaignWindow.CreatedCampaign;
                    MessageBox.Show("Campaign created successfully.", "Create Campaign");
                }
                else
                {
                    MessageBox.Show("Failed to create campaign.", "Create Campaign");
                }
            }
        }

        private void LoadCampaigns()
        {
            Campaigns.Clear(); // Clear existing items

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetAllCampaigns", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campaign campaign = new Campaign
                        {
                            CampaignID = Convert.ToInt32(reader["CampaignID"]),
                            CampaignName = reader["CampaignName"].ToString(),
                            HourlyRate = Convert.ToDecimal(reader["HourlyRates"]),
                            DailyRate = Convert.ToDecimal(reader["DailyRates"])
                        };

                        Campaigns.Add(campaign);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading campaigns: " + ex.Message);
            }
        }

        public void RateType_SelectionChanged(string selectedRateType)
        {
            if (selectedRateType == "Hourly" || selectedRateType == "Daily")
            {
                IsDurationVisible = true;
                DurationLabel = selectedRateType == "Hourly" ? "Hours:" : "Days:";
            }
            else
            {
                IsDurationVisible = false;
            }
        }
        private void SelectTalent()
        {
            SelectTalentWindow selectTalentWindow = new SelectTalentWindow();
            bool? result = selectTalentWindow.ShowDialog();

            if (result == true)
            {
                SelectedTalents = selectTalentWindow.SelectedTalents;
            }
        }


        private void CalculateBookingFee()
        {
            if (SelectedCampaign != null && !string.IsNullOrEmpty(RateType) && int.TryParse(Duration, out int duration) && SelectedTalents.Count > 0)
            {
                decimal rate = RateType == "Hourly" ? SelectedCampaign.HourlyRate
                    : SelectedCampaign.DailyRate;
                decimal bookingFee = rate * duration * SelectedTalents.Count;
                BookingFee = bookingFee.ToString("C");
            }
            else
            {
                MessageBox.Show("Please ensure all required fields are selected and filled in correctly.", "Validation Error");
            }
        }

        private void CreateBooking()
        {
            if (ValidateBookingDetails())
            {
                Booking newBooking = new Booking
                {
                    StaffID = int.Parse(StaffID),
                    ClientID = int.Parse(ClientId),
                    CampaignID = SelectedCampaign.CampaignID,
                    CampaignStartDate = CampaignStartDate.Value,
                    CampaignEndDate = CampaignEndDate.Value,
                    CampaignLocation = CampaignLocation,
                    SpecialRequirement = SpecialRequirement,
                    RateType = RateType,
                    Duration = int.Parse(Duration),
                    BookingFee = decimal.Parse(BookingFee, System.Globalization.NumberStyles.Currency),
                    TalentIDs = SelectedTalents.Select(t => t.TalentID).ToList()
                };

                BookingID = AddBooking(newBooking);

                if (BookingID != -1)
                {
                    MessageBox.Show($"Booking created successfully. Booking ID is: {BookingID}", "Create Booking");

                    // Clear all fields
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to create booking. Please try again.", "Create Booking");
                }
            }
        }


        private bool ValidateBookingDetails()
        {
            if (string.IsNullOrEmpty(ClientId) || SelectedCampaign == null ||
                !CampaignStartDate.HasValue || !CampaignEndDate.HasValue ||
                string.IsNullOrEmpty(CampaignLocation) || string.IsNullOrEmpty(RateType) ||
                (RateType == "Hourly" && string.IsNullOrEmpty(Duration)) ||
                SelectedTalents.Count == 0)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error");
                return false;
            }

            if (CampaignStartDate.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Campaign Start Date cannot be in the past.", "Validation Error");
                return false;
            }

            if (CampaignEndDate.Value < CampaignStartDate.Value)
            {
                MessageBox.Show("Campaign End Date cannot be earlier than Campaign Start Date.", "Validation Error");
                return false;
            }

            return true;
        }

        private int AddBooking(Booking newBooking)
        {
            int newBookingID = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddBooking", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ClientID", newBooking.ClientID);
                command.Parameters.AddWithValue("@CampaignID", newBooking.CampaignID);
                command.Parameters.AddWithValue("@StaffID", newBooking.StaffID);
                command.Parameters.AddWithValue("@CampaignStartDate", newBooking.CampaignStartDate.Date);
                command.Parameters.AddWithValue("@CampaignEndDate", newBooking.CampaignEndDate.Date);
                command.Parameters.AddWithValue("@CampaignLocation", newBooking.CampaignLocation);
                command.Parameters.AddWithValue("@SpecialRequirement", newBooking.SpecialRequirement);
                command.Parameters.AddWithValue("@RateType", newBooking.RateType);
                command.Parameters.AddWithValue("@Duration", newBooking.Duration);
                command.Parameters.AddWithValue("@BookingFee", newBooking.BookingFee);

                // Create table-valued parameter for talent IDs
                DataTable talentIDsTable = new DataTable();
                talentIDsTable.Columns.Add("TalentID", typeof(int));
                foreach (var talentID in newBooking.TalentIDs)
                {
                    talentIDsTable.Rows.Add(talentID);
                }
                SqlParameter talentIDsParam = command.Parameters.AddWithValue("@TalentIDs", talentIDsTable);
                talentIDsParam.SqlDbType = SqlDbType.Structured;
                talentIDsParam.TypeName = "dbo.IntList";

                // Output parameter for the new BookingID
                SqlParameter outputBookingID = new SqlParameter
                {
                    ParameterName = "@NewBookingID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputBookingID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve the new BookingID
                    newBookingID = (int)outputBookingID.Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error");
                }
            }

            return newBookingID;
        }



        private void ClearFields()
        {
            StaffID = string.Empty;
            ClientId = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            SelectedCampaign = null;
            CampaignStartDate = null;
            CampaignEndDate = null;
            CampaignLocation = string.Empty;
            SpecialRequirement = string.Empty;
            RateType = string.Empty;
            IsDurationVisible = false;
            Duration = string.Empty;
            BookingFee = string.Empty;
            SelectedTalents.Clear();
        }

    }
}
