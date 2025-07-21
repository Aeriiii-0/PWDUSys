using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Social_Blade_Dashboard
{
    public partial class userLogin : Window
    {
        private bool isUsernamePlaceholder = true;

        public userLogin()
        {
            InitializeComponent();

            // Set initial placeholder text
            UsernameTextBox.Text = "Username or email";
            UsernameTextBox.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isUsernamePlaceholder)
            {
                UsernameTextBox.Text = "";
                UsernameTextBox.Foreground = new SolidColorBrush(Colors.Black);
                isUsernamePlaceholder = false;
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameTextBox.Text = "Username or email";
                UsernameTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                isUsernamePlaceholder = true;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the entered credentials
            string username = isUsernamePlaceholder ? "" : UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Basic validation
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter your username or email.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Add your authentication logic here
            // For now, just show a success message
            MessageBox.Show($"Login attempt for: {username}", "Login Info", MessageBoxButton.OK, MessageBoxImage.Information);

            // Example: Navigate to main window
            // MainWindow mainWindow = new MainWindow();
            // mainWindow.Show();
            // this.Close();
        }
    }
}