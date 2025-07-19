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
                        // files
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

