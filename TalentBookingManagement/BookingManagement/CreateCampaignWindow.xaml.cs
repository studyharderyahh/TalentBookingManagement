using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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

namespace TalentBookingManagement.TalentManagement
{
    /// <summary>
    /// Interaction logic for CreateCampaignWindow.xaml
    /// </summary>
    public partial class CreateCampaignWindow : Window
    {
        public Campaign CreatedCampaign { get; private set; }
        public CreateCampaignWindow()
        {
            InitializeComponent();
        }

        private void CreateCampaign_Click(object sender, RoutedEventArgs e)
        {
            // Gather input values
            string campaignName = txtCampaignName.Text.Trim();
            decimal hourlyRates;
            decimal dailyRates;

            if (!decimal.TryParse(txtHourlyRates.Text.Trim(), out hourlyRates) || hourlyRates <= 0)
            {
                MessageBox.Show("Please enter a valid hourly rate greater than zero.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtDailyRates.Text.Trim(), out dailyRates) || dailyRates <= 0)
            {
                MessageBox.Show("Please enter a valid daily rate greater than zero.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Call stored procedure
            int newCampaignID = CreateCampaign(campaignName, hourlyRates, dailyRates);

            if (newCampaignID > 0)
            {
                CreatedCampaign = new Campaign
                {
                    CampaignID = newCampaignID,
                    CampaignName = campaignName,
                    HourlyRate = hourlyRates,
                    DailyRate = dailyRates
                };

                MessageBox.Show($"Campaign created successfully. New Campaign ID: {newCampaignID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();
            }
            else
            {
                MessageBox.Show("Failed to create campaign. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CreateCampaign(string campaignName, decimal hourlyRates, decimal dailyRates)
        {
            try
            {
                int newCampaignID = 0;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBMConnectionString"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("CreateCampaign", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CampaignName", campaignName);
                        command.Parameters.AddWithValue("@HourlyRates", hourlyRates);
                        command.Parameters.AddWithValue("@DailyRates", dailyRates);

                        // Add output parameter
                        SqlParameter outputIdParam = new SqlParameter("@NewCampaignID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Get the value of the output parameter
                        newCampaignID = (int)outputIdParam.Value;

                        connection.Close();
                    }
                }

                return newCampaignID;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating campaign: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        private void ClearFields()
        {
            txtCampaignName.Text = string.Empty;
            txtHourlyRates.Text = string.Empty;
            txtDailyRates.Text = string.Empty;
        }
    }
}
