﻿
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Google.Cloud.Firestore;
using MaterialDesignThemes.Wpf;

namespace Social_Blade_Dashboard
{
    public partial class userLogin : Window
    {
        private bool isUsernamePlaceholder = true;
        private bool isPasswordVisible = false;
        FirestoreDb db;
        public userLogin()
        {
            InitializeComponent();
            string path = AppDomain.CurrentDomain.BaseDirectory + "pwsd-7f264-firebase-adminsdk-fbsvc-93a2654d90.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("pwsd-7f264");
            // Set initial placeholder text
            UsernameTextBox.Text = "Username or email";
            UsernameTextBox.Foreground = new SolidColorBrush(Colors.Gray);
        }

     //user

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
                UsernameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)); // #666666
                isUsernamePlaceholder = true;
            }
        }
        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                // Query Firestore collection 'Users'
                Query usersQuery = db.Collection("Users")
                                     .WhereEqualTo("usermail", username)
                                     .WhereEqualTo("password", password);

                QuerySnapshot snapshot = await usersQuery.GetSnapshotAsync();

                if (snapshot.Documents.Count == 0)
                {
                    // No user found with given username & password
                    return null;
                }

                // Retrieve the first matched user document
                DocumentSnapshot userDoc = snapshot.Documents[0];

                // Retrieve the Role field (case-sensitive in Firestore)
                string role = userDoc.ContainsField("Role") ? userDoc.GetValue<string>("Role") : null;

                return role;  // Could be "user", "admin", etc.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error authenticating user: {ex.Message}", "Login Error");
                return null;
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow dashboardWindow = new MainWindow();
            dashboardWindow.Show();
            this.Close();
            //string username = isUsernamePlaceholder ? "" : UsernameTextBox.Text;
            //string password = PasswordBox.Password;

            //// Validate username
            //if (string.IsNullOrWhiteSpace(username))
            //{
            //    MessageBox.Show("Please enter your username or email.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            //    ResetUsernameField();
            //    PasswordBox.Clear();
            //    return;
            //}

            //// Validate password
            //if (string.IsNullOrWhiteSpace(password))
            //{
            //    MessageBox.Show("Please enter your password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            //    ResetUsernameField();
            //    PasswordBox.Clear();
            //    return;
            //}

            //try
            //{
            //    // Authenticate user using Firestore
            //    string role = await AuthenticateUserAsync(username, password);

            //    if (string.IsNullOrEmpty(role))
            //    {
            //        MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //        ResetUsernameField();
            //        PasswordBox.Clear();
            //        return;
            //    }

            //    // Login success
            //    MessageBox.Show($"Welcome, {username}! Your role is: {role}.", "Login Success", MessageBoxButton.OK, MessageBoxImage.Information);



            //    // TODO: Open different windows based on role
            //    // Example:
            //    // if (role == "Admin") new AdminDashboard().Show();
            //    // else new UserDashboard().Show();
            //    // this.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error during login: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }


        private void ResetUsernameField()
        {
            UsernameTextBox.Text = "Username or email";
            UsernameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)); // #666666
            isUsernamePlaceholder = true;
        }

        //pass
        private void TogglePasswordVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show plain text
                VisiblePasswordBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                VisiblePasswordBox.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
            }
            else
            {
                // Hide plain text
                PasswordBox.Password = VisiblePasswordBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
                VisiblePasswordBox.Text = PasswordBox.Password;
        }

        private void VisiblePasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
                PasswordBox.Password = VisiblePasswordBox.Text;
        }


        //custom top bar
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