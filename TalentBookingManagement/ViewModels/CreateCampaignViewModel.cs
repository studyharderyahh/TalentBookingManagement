using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using TalentBookingManagement.Models;

namespace TalentBookingManagement.ViewModels
{
    public class CreateCampaignViewModel : BaseViewModel
    {
        private string campaignName;
        private decimal hourlyRates;
        private decimal dailyRates;
        public RelayCommand CreateCampaignCommand { get; private set; }

        public CreateCampaignViewModel()
        {
            Logger.Log("CreateCampaignViewModel instantiated");
            CreateCampaignCommand = new RelayCommand(CreateCampaign, CanCreateCampaign);
        }

        public string CampaignName
        {
            get => campaignName;
            set
            {
                campaignName = value;
                OnPropertyChanged();
                Logger.Log($"CampaignName changed: {campaignName}");
                UpdateCanExecute();
            }
        }

        public decimal HourlyRates
        {
            get => hourlyRates;
            set
            {
                hourlyRates = value;
                OnPropertyChanged();
                Logger.Log($"HourlyRates changed: {hourlyRates}");
                UpdateCanExecute();
            }
        }

        public decimal DailyRates
        {
            get => dailyRates;
            set
            {
                dailyRates = value;
                OnPropertyChanged();
                Logger.Log($"DailyRates changed: {dailyRates}");
                UpdateCanExecute();
            }
        }

        private bool CanCreateCampaign()
        {
            bool hasValidName = !string.IsNullOrEmpty(CampaignName);
            bool hasValidHourlyRate = HourlyRates > 0;
            bool hasValidDailyRate = DailyRates > 0;

            bool canCreate = hasValidName && hasValidHourlyRate && hasValidDailyRate;

            Logger.Log($"CanCreateCampaign - hasValidName: {hasValidName}, hasValidHourlyRate: {hasValidHourlyRate}, hasValidDailyRate: {hasValidDailyRate}");
            Logger.Log($"CanCreateCampaign: {canCreate}");
            return canCreate;
        }

        private void UpdateCanExecute()
        {
            ((RelayCommand)CreateCampaignCommand).RaiseCanExecuteChanged();
        }

        private void CreateCampaign()
        {
            try
            {
                int newCampaignID = CreateCampaignInDatabase(CampaignName, HourlyRates, DailyRates);

                if (newCampaignID > 0)
                {
                    Logger.Log($"Campaign created successfully. New Campaign ID: {newCampaignID}");
                    MessageBox.Show($"Campaign created successfully. New Campaign ID: {newCampaignID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
                else
                {
                    Logger.Log("Failed to create campaign.");
                    MessageBox.Show("Failed to create campaign. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error creating campaign: {ex.Message}");
                MessageBox.Show($"Error creating campaign: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CreateCampaignInDatabase(string campaignName, decimal hourlyRates, decimal dailyRates)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand("CreateCampaign", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CampaignName", campaignName);
                command.Parameters.AddWithValue("@HourlyRates", hourlyRates);
                command.Parameters.AddWithValue("@DailyRates", dailyRates);
                var outputIdParam = new SqlParameter("@NewCampaignID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();
                return (int)outputIdParam.Value;
            }
        }

        private void ClearFields()
        {
            CampaignName = string.Empty;
            HourlyRates = 0;
            DailyRates = 0;
        }
    }
}
