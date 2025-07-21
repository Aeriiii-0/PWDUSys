using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        }
        // Example: Navigate to main window
        // MainWindow mainWindow = new MainWindow();
        // mainWindow.Show();
        // this.Close();


        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                IconMaxRestore.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                IconMaxRestore.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                HeaderRow.Height = new GridLength(35);
                HeaderPanel.Padding = new Thickness(3, 0, 3, 0);
            }
            else
            {
                HeaderRow.Height = new GridLength(60);
                HeaderPanel.Padding = new Thickness(5, 0, 5, 0);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Draggable window functionality
        private bool isDragging = false;
        private Point clickPosition;

        private void HeaderPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                isDragging = true;
                clickPosition = e.GetPosition(this);
                HeaderPanel.CaptureMouse();
                if (e.ClickCount == 2)
                {
                    BtnMaximizeRestore_Click(sender, null);
                }
            }
        }

        private void HeaderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(this);
                double deltaX = currentPosition.X - clickPosition.X;
                double deltaY = currentPosition.Y - clickPosition.Y;
                this.Left += deltaX;
                this.Top += deltaY;
            }
        }

        private void HeaderPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                HeaderPanel.ReleaseMouseCapture();
            }
        }

     




    }
}