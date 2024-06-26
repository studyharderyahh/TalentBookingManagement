using System.Windows;
using System.Windows.Controls;

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
                contextMenu.Items.Add(CreateMenuItem("Staff Option 1"));
                contextMenu.Items.Add(CreateMenuItem("Staff Option 2"));
                contextMenu.Items.Add(CreateMenuItem("Staff Option 3"));
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
