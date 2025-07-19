using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;

namespace Social_Blade_Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardUSC.xaml
    /// </summary>
    public partial class DashboardUSC : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection LastHourSeries { get; set; }
        public SeriesCollection LastHourSeries1 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public DashboardUSC()
        {
            InitializeComponent();

            // Bar chart (stacked column)
            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Title = "Positive",
                    Values = new ChartValues<double> { 25, 52, 61, 89 },
                    StackMode = StackMode.Values,
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Title = "Negative",
                    Values = new ChartValues<double> { -15, -75, -16, -49 },
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };

            // Line chart 1
            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Line A",
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(1),
                        new ObservableValue(9),
                        new ObservableValue(4),
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(1),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(7)
                    }
                }
            };

            // Line chart 2
            LastHourSeries1 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Line B",
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(13),
                        new ObservableValue(11),
                        new ObservableValue(9),
                        new ObservableValue(14),
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(12),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(7)
                    }
                }
            };

            Labels = new[] { "Feb 7", "Feb 8", "Feb 9", "Feb 10" };
            Formatter = value => value.ToString("N0");

            DataContext = this;
        }
    }
}
