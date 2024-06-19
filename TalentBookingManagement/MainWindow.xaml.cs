using System.Windows;
using System.Windows.Controls;
using TalentBookingManagement;
using TalentBookingManagement.BookingManagement;

namespace TalentManagementSystem
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
            string userName = "John Doe";  // 예시 사용자 이름
            string userId = "12345";       // 예시 사용자 ID
            userRole = "Admin";     // 예시 사용자 역할, 실제로는 로그인 시 받아와야 합니다.

            UserNameTextBlock.Text = $"User Name: {userName}";
            UserIDTextBlock.Text = $"User ID: {userId}";
            UserRoleTextBlock.Text = $"User Role: {userRole}";
        }

        private void SetMenuVisibility()
        {
            if (userRole == "Admin")
            {
                // Admin은 모든 메뉴 항목을 볼 수 있습니다.
                TalentButton.Visibility = Visibility.Visible;
            }
            else if (userRole == "Booking Agent")
            {
                // Booking Agent는 일부 메뉴 항목에만 접근 가능합니다.
                TalentButton.Visibility = Visibility.Visible;
            }
            else if (userRole == "Assistant")
            {
                // Assistant는 제한된 메뉴 항목에만 접근 가능합니다.
                TalentButton.Visibility = Visibility.Visible;
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            ContextMenu contextMenu = new ContextMenu();

            if (clickedButton.Name == "BookingButton")
            {
                contextMenu.Items.Add(CreateMenuItem("Add Booking"));
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
            MenuItem menuItem = new MenuItem();
            menuItem.Header = header;
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
            else if (buttonText == "Add Booking")
            {
                AddBookingWindow addBookingWindow = new AddBookingWindow();
                addBookingWindow.Show();
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
