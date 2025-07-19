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
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                    break;

                case "Add Convict":
                    var addConvictForm = new UEF_AddConvict
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 700,
                        Height = 800
                    };

                    var wrapper = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(10),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")),
                        BorderThickness = new Thickness(3),
                        Padding = new Thickness(20),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Child = addConvictForm,
                        Effect = new System.Windows.Media.Effects.DropShadowEffect
                        {
                            BlurRadius = 12,
                            ShadowDepth = 2,
                            Direction = 270,
                            Color = Colors.Black,
                            Opacity = 0.2
                        }
                    };

                    ContentArea.Children.Add(wrapper);
                    break;

                default:
                    ContentArea.Children.Add(new TextBlock
                    {
                        Text = "Unknown Tab",
                        FontSize = 16,
                        Foreground = Brushes.Gray,
                        HorizontalAlignment = HorizontalAlignment.Center,
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
