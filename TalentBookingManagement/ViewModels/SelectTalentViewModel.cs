using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;
using System.Collections.Generic;

namespace TalentBookingManagement.ViewModels
{
    public class SelectTalentViewModel : BaseViewModel
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString;

        private string selectedAvailabilityStatus;
        private string availabilityStatus;
        private string skillName;
        private DateTime? bookingStartDate;
        private DateTime? bookingEndDate;
        private string preferredEngagement;

        public SelectTalentViewModel()
        {
            AvailabilityStatuses = new ObservableCollection<string>
            {
                "Available for booking",
                "booked",
                "Unavailable"
            };

            SearchCommand = new RelayCommand(SearchTalent);
            SelectCommand = new RelayCommand(SelectTalent);
            CloseCommand = new RelayCommand(CloseWindow);
            AvailabilityStatusChangedCommand = new RelayCommand(OnAvailabilityStatusChanged);

            LoadInitialTalents();
        }

        public ObservableCollection<Talent> Talents { get; } = new ObservableCollection<Talent>();
        public ObservableCollection<Talent> SelectedTalents { get; private set; } = new ObservableCollection<Talent>();
        public ObservableCollection<string> AvailabilityStatuses { get; }

        public string SelectedAvailabilityStatus
        {
            get => selectedAvailabilityStatus;
            set
            {
                if (SetProperty(ref selectedAvailabilityStatus, value))
                {
                    OnAvailabilityStatusChanged();
                }
            }
        }

        public string AvailabilityStatus
        {
            get => availabilityStatus;
            set
            {
                availabilityStatus = value;
                OnPropertyChanged(nameof(AvailabilityStatus));
            }
        }

        public string SkillName
        {
            get => skillName;
            set => SetProperty(ref skillName, value);
        }

        public DateTime? BookingStartDate
        {
            get => bookingStartDate;
            set => SetProperty(ref bookingStartDate, value);
        }

        public DateTime? BookingEndDate
        {
            get => bookingEndDate;
            set => SetProperty(ref bookingEndDate, value);
        }

        public string PreferredEngagement
        {
            get => preferredEngagement;
            set => SetProperty(ref preferredEngagement, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand AvailabilityStatusChangedCommand { get; }

        public event Action CloseRequested;

        private void LoadInitialTalents()
        {
            Logger.Info("Loading initial talents.");
            Talents.Clear();
            var initialTalents = FetchFilteredTalents(null, null, null, null, null);
            foreach (var talent in initialTalents)
            {
                Talents.Add(talent);
            }
        }
        private void OnAvailabilityStatusChanged()
        {
            // Handle the change in availability status here
            Console.WriteLine($"Availability Status changed to: {SelectedAvailabilityStatus}");
        }

        private void SearchTalent()
        {
            Logger.Info($"Searching talents with filters - AvailabilityStatus: {SelectedAvailabilityStatus}, SkillName: {skillName}, BookingStartDate: {bookingStartDate}, BookingEndDate: {bookingEndDate}, PreferredEngagement: {preferredEngagement}");
            Talents.Clear();
            var filteredTalents = FetchFilteredTalents(
                SelectedAvailabilityStatus, skillName, bookingStartDate, bookingEndDate, preferredEngagement);
            foreach (var talent in filteredTalents)
            {
                Talents.Add(talent);
            }
        }

        private void SelectTalent()
        {
            Logger.Info("Selecting talents.");
            SelectedTalents.Clear();
            foreach (var talent in Talents.Where(t => t.IsSelected))
            {
                SelectedTalents.Add(talent);
            }

            // Setting DialogResult to true to indicate success
            // var DialogResult = true;
            CloseRequested?.Invoke();
        }

        private void CloseWindow()
        {
            Logger.Info("Closing window.");
            CloseRequested?.Invoke();
        }

        private List<Talent> FetchFilteredTalents(string availabilityStatus, string skillName, DateTime? bookingStartDate, DateTime? bookingEndDate, string preferredEngagement)
        {
            var talents = new List<Talent>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand("spGetFilteredTalent", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@AvailabilityStatus", string.IsNullOrEmpty(availabilityStatus) ? (object)DBNull.Value : availabilityStatus);
                    cmd.Parameters.AddWithValue("@SkillName", string.IsNullOrEmpty(skillName) ? (object)DBNull.Value : skillName);
                    cmd.Parameters.AddWithValue("@BookingStartDate", bookingStartDate.HasValue ? (object)bookingStartDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@BookingEndDate", bookingEndDate.HasValue ? (object)bookingEndDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PreferredEngagement", string.IsNullOrEmpty(preferredEngagement) ? (object)DBNull.Value : preferredEngagement);

                    connection.Open();
                    Logger.Info("Executing stored procedure spGetFilteredTalent.");
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var talent = new Talent
                        {
                            TalentID = rdr["TalentID"] != DBNull.Value ? Convert.ToInt32(rdr["TalentID"]) : 0,
                            FirstName = rdr["FirstName"] != DBNull.Value ? rdr["FirstName"].ToString() : string.Empty,
                            LastName = rdr["LastName"] != DBNull.Value ? rdr["LastName"].ToString() : string.Empty,
                            AvailabilityStatus = rdr["AvailabilityStatus"] != DBNull.Value ? rdr["AvailabilityStatus"].ToString() : string.Empty,
                            PhoneNumber = rdr["PhoneNumber"] != DBNull.Value ? rdr["PhoneNumber"].ToString() : string.Empty,
                            Email = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString() : string.Empty,
                            City = rdr["City"] != DBNull.Value ? rdr["City"].ToString() : string.Empty,
                            Suburb = rdr["Suburb"] != DBNull.Value ? rdr["Suburb"].ToString() : string.Empty,
                            StreetAddress = rdr["StreetAddress"] != DBNull.Value ? rdr["StreetAddress"].ToString() : string.Empty,
                            Postcode = rdr["Postcode"] != DBNull.Value ? rdr["Postcode"].ToString() : string.Empty,
                            Skill = rdr["Skill"] != DBNull.Value ? rdr["Skill"].ToString() : string.Empty,
                            BookingID = rdr["BookingID"] != DBNull.Value ? Convert.ToInt32(rdr["BookingID"]) : 0,
                            BookingStartDate = rdr["BookingStartDate"] != DBNull.Value ? (DateTime)rdr["BookingStartDate"] : default,
                            BookingEndDate = rdr["BookingEndDate"] != DBNull.Value ? (DateTime)rdr["BookingEndDate"] : default,
                            SpecialRequirement = rdr["SpecialRequirement"] != DBNull.Value ? rdr["SpecialRequirement"].ToString() : string.Empty,
                            CampaignLocation = rdr["CampaignLocation"] != DBNull.Value ? rdr["CampaignLocation"].ToString() : string.Empty,
                            PreferredEngagement = rdr["PreferredEngagement"] != DBNull.Value ? rdr["PreferredEngagement"].ToString() : string.Empty
                        };

                        talents.Add(talent);
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error fetching talents: {ex.Message}");
                MessageBox.Show("Error fetching talents: " + ex.Message);
            }

            return talents;
        }
    }
}
