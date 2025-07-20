using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Social_Blade_Dashboard
{
    public partial class UserEntryForm : UserControl
    {
        public UserEntryForm()
        {
            InitializeComponent();
            SetActiveTab(ViewConvictsBtn); 
            SearchButton.IsEnabled = false; 
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchButton.IsEnabled = !string.IsNullOrWhiteSpace(SearchTextBox.Text);
        }

        private void PerformSearch()
        {
            string searchText = SearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                Console.WriteLine($"Searching for: {searchText}");
                OnSearchPerformed(searchText);
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                SetActiveTab(clickedButton);
            }
        }

        private void SetActiveTab(Button activeButton)
        {
            ViewConvictsBtn.Tag = null;
            EditInfoBtn.Tag = null;
            AddConvictBtn.Tag = null;

           
            activeButton.Tag = "Active";

            
            LoadTabContent(activeButton.Content.ToString());
        }

        private void LoadTabContent(string tabName)
        {
            ContentArea.Children.Clear();

            switch (tabName)
            {
                case "View Convicts":
                    ContentArea.Children.Add(new TextBlock
                    {
                        Text = "Data table goes here",
                        FontSize = 16,
                        Foreground = Brushes.Gray,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    break;

                case "Edit Info":
                    ContentArea.Children.Add(new TextBlock
                    {
                        Text = "Edit Info Content",
                        FontSize = 16,
                        Foreground = Brushes.Gray,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    break;

                case "Add Convict":
                    var addConvictForm = new UEF_AddConvict
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 550,
                        Height = 815
                    };

                    var wrapper = new Border
                    {
                        Background = (Brush)new BrushConverter().ConvertFromString("#f6f6f8"),
                        Padding = new Thickness(20),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Child = addConvictForm,
                    };

                    ContentArea.Children.Add(wrapper);
                    break;

                default:
                    ContentArea.Children.Add(new TextBlock
                    {
                        Text = "Unknown Tab",
                        FontSize = 16,
                        Foreground = Brushes.Gray,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    break;
            }
        }

        public void SetSearchText(string text)
        {
            SearchTextBox.Text = text;
        }

        public string GetSearchText()
        {
            return SearchTextBox.Text;
        }

        public event EventHandler<string> SearchPerformed;

        private void OnSearchPerformed(string searchText)
        {
            SearchPerformed?.Invoke(this, searchText);
        }
    }
}
