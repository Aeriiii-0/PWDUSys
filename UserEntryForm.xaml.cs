using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Social_Blade_Dashboard
{
    public partial class UserEntryForm : UserControl
    {
        private UIElement backgroundContent;

        public UserEntryForm()
        {
            InitializeComponent();
            SetActiveTab(ViewConvictsBtn);
            SearchButton.IsEnabled = false;
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
                    ContentArea.Children.Add(CreateConvictRecordsView());
                    break;

                case "Edit Info":
                    ContentArea.Children.Add(CreateConvictRecordsView());
                    break;

                case "Add Convict":
                    ShowAddConvictModal();
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

        private void ShowAddConvictModal()
        {
            backgroundContent = CreateConvictRecordsView();

            var blurEffect = new BlurEffect
            {
                Radius = 8,
                RenderingBias = RenderingBias.Performance
            };
            backgroundContent.Effect = blurEffect;
            backgroundContent.Opacity = 0.3;

            var addConvictForm = new UEF_AddConvict
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var modalWrapper = new Border
            {
                Background = Brushes.White,
                Padding = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = addConvictForm
            };

            ContentArea.Children.Add(backgroundContent);
            ContentArea.Children.Add(modalWrapper);
        }

        private void CloseModal_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundContent != null)
            {
                backgroundContent.Effect = null;
                backgroundContent.Opacity = 1.0;
            }

            ContentArea.Children.Clear();
            if (backgroundContent != null)
            {
                ContentArea.Children.Add(backgroundContent);
            }

            SetActiveTab(ViewConvictsBtn);
        }

        private UIElement CreateConvictRecordsView()
        {

            var container = new Grid();
            container.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


            var headerBorder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFromString("#F8F9FA"),
                BorderBrush = (Brush)new BrushConverter().ConvertFromString("#E5E7EB"),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(20, 15, 20, 15)
            };

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var headerText = new TextBlock
            {
                Text = "Convict Records",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#374151"),
                VerticalAlignment = VerticalAlignment.Center
            };
            headerGrid.Children.Add(headerText);
            Grid.SetColumn(headerText, 0);

            var refreshButton = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Padding = new Thickness(8),
                ToolTip = "Refresh",
            };
            refreshButton.Click += RefreshButton_Click;

            var refreshStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            var refreshIcon = new System.Windows.Shapes.Path
            {
                Data = Geometry.Parse("M4 4V20L10.5 14L20 20V4H4Z"),
                Fill = (Brush)new BrushConverter().ConvertFromString("#065F46"),
                Width = 14,
                Height = 14,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 0, 5, 0)
            };
            refreshStack.Children.Add(refreshIcon);

            var refreshText = new TextBlock
            {
                Text = "Refresh",
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#065F46"),
                VerticalAlignment = VerticalAlignment.Center
            };
            refreshStack.Children.Add(refreshText);

            refreshButton.Content = refreshStack;

            headerGrid.Children.Add(refreshButton);
            Grid.SetColumn(refreshButton, 1);

            headerBorder.Child = headerGrid;

            var dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                GridLinesVisibility = DataGridGridLinesVisibility.None,
                Background = Brushes.White,
                RowHeaderWidth = 0,
                CanUserResizeRows = false,
                CanUserReorderColumns = true,
                CanUserSortColumns = true,
                SelectionMode = DataGridSelectionMode.Single,
                SelectionUnit = DataGridSelectionUnit.FullRow,
                MinRowHeight = 40,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ColumnHeaderStyle = (Style)FindResource("DataGridHeaderStyle"),
                RowStyle = (Style)FindResource("DataGridRowStyle"),
                CellStyle = (Style)FindResource("DataGridCellStyle"),
                Margin = new Thickness(0)
            };

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Photo", Binding = new System.Windows.Data.Binding("Photo"), Width = 60, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new System.Windows.Data.Binding("LastName"), Width = 120, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new System.Windows.Data.Binding("FirstName"), Width = 120, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Middle Name", Binding = new System.Windows.Data.Binding("MiddleName"), Width = 120, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Extension Name", Binding = new System.Windows.Data.Binding("ExtensionName"), Width = 100, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Gender", Binding = new System.Windows.Data.Binding("Gender"), Width = 80, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Birth Month", Binding = new System.Windows.Data.Binding("BirthMonth"), Width = 90, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Birth Day", Binding = new System.Windows.Data.Binding("BirthDay"), Width = 70, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Birth Year", Binding = new System.Windows.Data.Binding("BirthYear"), Width = 90, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Contact Number", Binding = new System.Windows.Data.Binding("ContactNumber"), Width = 120, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Barangay", Binding = new System.Windows.Data.Binding("Barangay"), Width = 150, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Address", Binding = new System.Windows.Data.Binding("Address"), Width = 250, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Criminal Case", Binding = new System.Windows.Data.Binding("CriminalCase"), Width = 200, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Offense Committed", Binding = new System.Windows.Data.Binding("OffenseCommitted"), Width = 200, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Description", Binding = new System.Windows.Data.Binding("Description"), Width = 250, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Court Number", Binding = new System.Windows.Data.Binding("CourtNumber"), Width = 150, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Committing Court", Binding = new System.Windows.Data.Binding("CommittingCourt"), Width = 150, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Month", Binding = new System.Windows.Data.Binding("AdmittedMonth"), Width = 100, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Day", Binding = new System.Windows.Data.Binding("AdmittedDay"), Width = 80, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Year", Binding = new System.Windows.Data.Binding("AdmittedYear"), Width = 100, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Status Of Case", Binding = new System.Windows.Data.Binding("StatusOfCase"), Width = 150, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Graduated Month", Binding = new System.Windows.Data.Binding("GraduatedMonth"), Width = 110, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Graduated Day", Binding = new System.Windows.Data.Binding("GraduatedDay"), Width = 90, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Graduated Year", Binding = new System.Windows.Data.Binding("GraduatedYear"), Width = 110, IsReadOnly = true });

            dataGrid.ItemsSource = LoadConvicts();


            container.Children.Add(headerBorder);
            Grid.SetRow(headerBorder, 0);

            container.Children.Add(dataGrid);
            Grid.SetRow(dataGrid, 1);

            return container;
        }

        private List<Convict> LoadConvicts()
        {
            return new List<Convict>();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentArea.Children.Count > 0 && ContentArea.Children[0] is Grid container)
            {
                foreach (var child in container.Children)
                {
                    if (child is DataGrid dg)
                    {
                        dg.ItemsSource = LoadConvicts();
                        break;
                    }
                }
            }
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
            }
        }
    }


    public class Convict
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ExtensionName { get; set; }
        public string Gender { get; set; }
        public string BirthMonth { get; set; }
        public string BirthDay { get; set; }
        public string BirthYear { get; set; }
        public string ContactNumber { get; set; }
        public string Barangay { get; set; }
        public string Address { get; set; }
        public string CriminalCase { get; set; }
        public string OffenseCommitted { get; set; }
        public string Description { get; set; }
        public string CourtNumber { get; set; }
        public string CommittingCourt { get; set; }
        public string AdmittedMonth { get; set; }
        public string AdmittedDay { get; set; }
        public string AdmittedYear { get; set; }
        public string StatusOfCase { get; set; }
        public string GraduatedMonth { get; set; }
        public string GraduatedDay { get; set; }
        public string GraduatedYear { get; set; }
    }
}