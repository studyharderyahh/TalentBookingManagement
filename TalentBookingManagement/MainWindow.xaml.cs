using System.Windows;
using System.Windows.Controls;
using TalentBookingManagement.Staff_ClientManagement;
using TalentBookingManagement.Views;
using TalentBookingManagement.Views.Staff_ClientManagement;

namespace TalentBookingManagement
{
    public partial class MainWindow : Window
    {
        private string userRole;

        public MainWindow()
        {
            InitializeComponent();
            LoadUserData();
            SetMenuVisibility();
        }

        private void LoadUserData()
        {
            string userName = "John Doe";
            string userId = "12345";
            userRole = "Admin";

            UserNameTextBlock.Text = $"User Name: {userName}";
            UserIDTextBlock.Text = $"User ID: {userId}";
            UserRoleTextBlock.Text = $"User Role: {userRole}";
        }

        private void SetMenuVisibility()
        {
            if (userRole == "Admin")
            {
                TalentButton.Visibility = Visibility.Visible;
            }
            else if (userRole == "Booking Agent")
            {
                TalentButton.Visibility = Visibility.Visible;
            }
            else if (userRole == "Assistant")
            {
                TalentButton.Visibility = Visibility.Visible;
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            ContextMenu contextMenu = new ContextMenu();

            if (clickedButton.Name == "BookingButton")
            {
                contextMenu.Items.Add(CreateMenuItem("Make Booking"));
                contextMenu.Items.Add(CreateMenuItem("Update Booking"));
                contextMenu.Items.Add(CreateMenuItem("View Booking"));
                contextMenu.Items.Add(CreateMenuItem("Cancel Booking"));
                contextMenu.Items.Add(CreateMenuItem("Make Payment"));
                contextMenu.Items.Add(CreateMenuItem("Get Invoice"));
            }
            else if (clickedButton.Name == "StaffButton")
            {
                contextMenu.Items.Add(CreateMenuItem("Add New Staff"));
                contextMenu.Items.Add(CreateMenuItem("View Staff Details"));
                contextMenu.Items.Add(CreateMenuItem("View Staff Permission"));
                contextMenu.Items.Add(CreateMenuItem("Update Staff"));
                contextMenu.Items.Add(CreateMenuItem("Delete Staff"));
            }
            else if (clickedButton.Name == "ClientButton")
            {
                contextMenu.Items.Add(CreateMenuItem("View Client Details"));            
                contextMenu.Items.Add(CreateMenuItem("Add New Client"));
                contextMenu.Items.Add(CreateMenuItem("Update Client"));
                
            }
            else if (clickedButton.Name == "TalentButton")
            {
                if (userRole == "Admin" || userRole == "Booking Agent")
                {
                    contextMenu.Items.Add(CreateMenuItem("Add new talent"));
                }

                if (userRole == "Admin" || userRole == "Booking Agent" || userRole == "Assistant")
                {
                    contextMenu.Items.Add(CreateMenuItem("Activate talent"));
                    contextMenu.Items.Add(CreateMenuItem("Inactivate talent"));
                    contextMenu.Items.Add(CreateMenuItem("Searching with filter"));
                    contextMenu.Items.Add(CreateMenuItem("Read talent list"));
                }
            }

            clickedButton.ContextMenu = contextMenu;
            contextMenu.IsOpen = true;
        }

        private MenuItem CreateMenuItem(string header)
        {
            MenuItem menuItem = new MenuItem
            {
                Header = header
            };
            menuItem.Click += ChildMenuButton_Click;
            return menuItem;
        }

        private void ChildMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;
            string buttonText = clickedItem.Header.ToString();

            if (buttonText == "Add new talent")
            {
                AddTalentWindow addTalentWindow = new AddTalentWindow();
                addTalentWindow.Show();
            }
            else if (buttonText == "Activate talent")
            {
                ActiveTalentWindow activateTalentWindow = new ActiveTalentWindow();
                activateTalentWindow.Show();
            }
            else if (buttonText == "Inactivate talent")
            {
                InactiveTalentWindow inactivateTalentWindow = new InactiveTalentWindow();
                inactivateTalentWindow.Show();
            }
            else if (buttonText == "Searching with filter")
            {
                SearchingTalentWindow searchingTalentWindow = new SearchingTalentWindow();
                searchingTalentWindow.Show();
            }
            else if (buttonText == "Read talent list")
            {
                ReadTalentListWindow readTalentListWindow = new ReadTalentListWindow();
                readTalentListWindow.Show();
            }
            else if (buttonText == "Make Booking")
            {
                MakeBookingWindow makeBookingWindow = new MakeBookingWindow();
                makeBookingWindow.Show();
            }
            else if (buttonText == "Update Booking")
            {
                UpdateBookingWindow updateBookingWindow = new UpdateBookingWindow();
                updateBookingWindow.Show();
            }
            else if (buttonText == "View Booking")
            {
                ViewBookingsWindow viewAllBookingsWindow = new ViewBookingsWindow();
                viewAllBookingsWindow.Show();
            }
            else if (buttonText == "Cancel Booking")
            {
                CancelBookingWindow cancelBookingWindow = new CancelBookingWindow();
                cancelBookingWindow.Show();
            }

            else if (buttonText == "Add New Staff")
            {
                AddNewStaff addNewStaff = new AddNewStaff();
                addNewStaff.Show();
            }

            else if (buttonText == "View Staff Details")
            {
                ViewStaffDetails viewStaffDetails = new ViewStaffDetails();
                viewStaffDetails.Show();
            }

            else if (buttonText == "View Staff Permission")
            {
                ViewStaffPermission viewStaffPermission = new ViewStaffPermission();
                viewStaffPermission.Show();
            }

            else if (buttonText == "Update Staff")
            {
                UpdateSatff updateStaff = new UpdateSatff();
                updateStaff.Show();
            }

            else if (buttonText == "Delete Staff")
            {
                DeleteStaff deleteStaff = new DeleteStaff();
                deleteStaff.Show();
            }

            else if (buttonText == "View Client Details")
            {
                ViewClientDetails viewClientDetails = new ViewClientDetails();
                viewClientDetails.Show();
            }

            else if (buttonText == "Add New Client")
            {
                AddNewClient addNewClient = new AddNewClient();
                addNewClient.Show();
            }

            else if (buttonText == "Update Client")
            {
                UpdateClient updateClient = new UpdateClient();
                updateClient.Show();
            }
            else
            {
                MessageBox.Show($"Navigating to: {buttonText}");
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logged out!");
        }
    }
}
