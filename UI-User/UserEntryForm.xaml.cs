using Business_Layer;
using Common_Class;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private Entry_Business_Logic BusinessLogic = new Entry_Business_Logic();

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

        private async void LoadTabContent(string tabName)
        {
            ContentArea.Children.Clear();

            switch (tabName)
            {
                case "View Convicts":
                    ContentArea.Children.Add(await CreateConvictRecordsView());
                    break;

                case "Edit Info":
                    ContentArea.Children.Add(await CreateConvictRecordsView());
                    break;

                case "Add Convict":
                    ShowAddConvictModal();
                    SearchButton.IsEnabled = false;
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

        private async void ShowAddConvictModal()
        {
            backgroundContent = await CreateConvictRecordsView();

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

        private async Task<UIElement> CreateConvictRecordsView()
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
                Data = Geometry.Parse("M17.65,6.35C16.2,4.9 14.21,4 12,4C7.58,4 4,7.58 4,12H1l4,4 4-4H6c0-3.31 2.69-6 6-6 " +
                          "c1.66,0 3.14,0.69 4.22,1.78L13,11h7V4l-2.35,2.35z"),
                Fill = (Brush)new BrushConverter().ConvertFromString("#065F46"),
                Width = 16,
                Height = 16,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center
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
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Case Number", Binding = new System.Windows.Data.Binding("caseId"), Width = 200, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Barangay", Binding = new System.Windows.Data.Binding("barangay"), Width = 150, IsReadOnly = true });

            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Photo", Binding = new System.Windows.Data.Binding("Photo"), Width = 60, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new System.Windows.Data.Binding("lastName"), Width = 120, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new System.Windows.Data.Binding("firstName"), Width = 120, IsReadOnly = true });
            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Middle Name", Binding = new System.Windows.Data.Binding("middleName"), Width = 120, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Extension Name", Binding = new System.Windows.Data.Binding("ExtensionName"), Width = 100, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Gender", Binding = new System.Windows.Data.Binding("Gender"), Width = 80, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Offense Committed", Binding = new System.Windows.Data.Binding("offenseCommitted"), Width = 200, IsReadOnly = true });


            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Birth Month", Binding = new System.Windows.Data.Binding("BirthMonth"), Width = 90, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Birth Day", Binding = new System.Windows.Data.Binding("BirthDay"), Width = 70, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Birth Year", Binding = new System.Windows.Data.Binding("BirthYear"), Width = 90, IsReadOnly = true });

            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Contact Number", Binding = new System.Windows.Data.Binding("ContactNumber"), Width = 120, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Address", Binding = new System.Windows.Data.Binding("Address"), Width = 250, IsReadOnly = true });

            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Criminal Case", Binding = new System.Windows.Data.Binding("CriminalCase"), Width = 200, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Description", Binding = new System.Windows.Data.Binding("Description"), Width = 250, IsReadOnly = true });

            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Court Number", Binding = new System.Windows.Data.Binding("CourtNumber"), Width = 150, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Committing Court", Binding = new System.Windows.Data.Binding("CommittingCourt"), Width = 150, IsReadOnly = true });


            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Date", Binding = new System.Windows.Data.Binding("dateAdmitted"), Width = 110, IsReadOnly = true });

            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Month", Binding = new System.Windows.Data.Binding("AdmittedMonth"), Width = 100, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Day", Binding = new System.Windows.Data.Binding("AdmittedDay"), Width = 80, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Admitted Year", Binding = new System.Windows.Data.Binding("AdmittedYear"), Width = 100, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Status Of Case", Binding = new System.Windows.Data.Binding("status"), Width = 150, IsReadOnly = true });

            dataGrid.Columns.Add(new DataGridTextColumn { Header = "Date Graduated", Binding = new System.Windows.Data.Binding("dateGraduated"), Width = 110, IsReadOnly = true });


            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Graduated Month", Binding = new System.Windows.Data.Binding("GraduatedMonth"), Width = 110, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Graduated Day", Binding = new System.Windows.Data.Binding("GraduatedDay"), Width = 90, IsReadOnly = true });
            //dataGrid.Columns.Add(new DataGridTextColumn { Header = "Graduated Year", Binding = new System.Windows.Data.Binding("GraduatedYear"), Width = 110, IsReadOnly = true });

            dataGrid.ItemsSource = await LoadEntryAsync();


            container.Children.Add(headerBorder);
            Grid.SetRow(headerBorder, 0);

            container.Children.Add(dataGrid);
            Grid.SetRow(dataGrid, 1);

            return container;
        }

        private async Task<List<Entry>> LoadEntryAsync()
        {
            //IMPORTANT change the barangay here barangGAYS
            string Barangay = "Poblacion";
            var entry = BusinessLogic.GetAllEntriesByBarangay(Barangay);
            return await entry;
        }
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Children.Clear();
            ContentArea.Children.Add(await CreateConvictRecordsView());
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
}
