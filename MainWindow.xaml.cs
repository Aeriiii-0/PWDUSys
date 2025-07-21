using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Social_Blade_Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isSidebarExpanded = false;

        public MainWindow()
        {
            InitializeComponent();
            MenuList.SelectedIndex = 0;
        }

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

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


        private void BtnMenuToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isSidebarExpanded)
            {
                Sidebar.Width = 200;

                foreach (var item in MenuList.Items)
                {
                    if (item is ListBoxItem listBoxItem)
                    {
                        if (listBoxItem.Content is StackPanel panel)
                        {
                            foreach (var child in panel.Children)
                            {
                                if (child is TextBlock textBlock)
                                    textBlock.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
            }
            else
            {
                Sidebar.Width = 70;

                foreach (var item in MenuList.Items)
                {
                    if (item is ListBoxItem listBoxItem)
                    {
                        if (listBoxItem.Content is StackPanel panel)
                        {
                            foreach (var child in panel.Children)
                            {
                                if (child is TextBlock textBlock)
                                    textBlock.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
            }

            isSidebarExpanded = !isSidebarExpanded;
        }

        //movable frame
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

        private void HeaderPanel_MouseLeftButtonDown_Simple(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    BtnMaximizeRestore_Click(sender, null);
                }
                else
                {
                    this.DragMove();
                }
            }
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListBoxItem selectedItem)
            {
                string tag = selectedItem.Tag.ToString();

                
                RenderPages.Children.Clear();

                
                switch (tag)
                {
                    case "Dashboard":
                        RenderPages.Children.Add(new DashboardUSC());
                        break;

                    case "Accounts":
                        RenderPages.Children.Add(new UserEntryForm());
                        break;

                    case "Files":
                        RenderPages.Children.Add(new UserProfile());
                        break;

                    case "Notifications":
                        //notifs
                        break;

                    case "Analytics":
                        //analytics
                        break;

                    case "Settings":
                        //call settngs
                        break;

                    case "Logout":
                        Application.Current.Shutdown();
                        break;
                }
            }
        }

    }
}

