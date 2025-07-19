using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Social_Blade_Dashboard
{
    public partial class DashboardUSC : UserControl
    {
        public DashboardUSC()
        {
            InitializeComponent();
            UpdateTaskCount();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskPanel.Visibility = Visibility.Visible;
            NewTaskTextBox.Focus();
        }

        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string taskText = NewTaskTextBox.Text.Trim();

            if (string.IsNullOrEmpty(taskText) || taskText == "Enter your note here...")
            {
                MessageBox.Show("Please enter a valid note.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CreateNewTaskItem(taskText);

            NewTaskTextBox.Text = "Enter your note here...";
            NewTaskTextBox.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
            NewTaskPanel.Visibility = Visibility.Collapsed;

            UpdateTaskCount();
        }

        private void CancelTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskTextBox.Text = "Enter your note here...";
            NewTaskTextBox.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
            NewTaskPanel.Visibility = Visibility.Collapsed;
        }

        private void NewTaskTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NewTaskTextBox.Text == "Enter your note here...")
            {
                NewTaskTextBox.Text = "";
                NewTaskTextBox.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            }
        }

        private void NewTaskTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewTaskTextBox.Text))
            {
                NewTaskTextBox.Text = "Enter your note here...";
                NewTaskTextBox.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
            }
        }

        private void NewTaskTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveTaskButton_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                CancelTaskButton_Click(sender, e);
            }
        }

        private void TaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Parent is Grid grid && grid.Parent is Border border)
            {
                TasksContainer.Children.Remove(border);
                UpdateTaskCount();
            }
        }

        private void TaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Optional logic if a task is unchecked.
            // You can leave this empty or add visual feedback here.
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Parent is Grid grid && grid.Parent is Border border)
            {
                var result = MessageBox.Show("Are you sure you want to delete this note?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TasksContainer.Children.Remove(border);
                    UpdateTaskCount();
                }
            }
        }

        private void CreateNewTaskItem(string taskText)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(249, 249, 249)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var checkBox = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 2, 15, 0)
            };
            checkBox.Checked += TaskCheckBox_Checked;
            checkBox.Unchecked += TaskCheckBox_Unchecked;
            Grid.SetColumn(checkBox, 0);

            var textBlock = new TextBlock
            {
                Text = taskText,
                FontWeight = FontWeights.Medium,
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(textBlock, 1);

            var deleteButton = new Button
            {
                Content = "×",
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153)),
                FontSize = 16,
                Width = 20,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top,
                ToolTip = "Delete note"
            };
            deleteButton.Click += DeleteTaskButton_Click;
            Grid.SetColumn(deleteButton, 2);

            grid.Children.Add(checkBox);
            grid.Children.Add(textBlock);
            grid.Children.Add(deleteButton);

            border.Child = grid;

            TasksContainer.Children.Insert(0, border);
        }

        private void UpdateTaskCount()
        {
            int taskCount = TasksContainer.Children.Count;
            // Optionally bind or update a counter UI element here
        }
    }
}
