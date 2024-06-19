using System.Windows;

namespace TalentManagementSystem
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = UserIDTextBox.Text;
            string password = PasswordBox.Password;

            // 예시로 사용하는 하드코딩된 사용자 데이터베이스
            if (ValidateUser(userId, password, out string userRole))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorMessageTextBlock.Text = "Invalid User ID or Password";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private bool ValidateUser(string userId, string password, out string userRole)
        {

            userRole = string.Empty;

            if (userId == "admin" && password == "admin")
            {
                userRole = "Admin";
                return true;
            }
            else if (userId == "agent" && password == "agent")
            {
                userRole = "Booking Agent";
                return true;
            }
            else if (userId == "assistant" && password == "assistant")
            {
                userRole = "Assistant";
                return true;
            }
            return false;
        }
    }
}
