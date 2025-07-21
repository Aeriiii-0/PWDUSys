using LiveCharts;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Social_Blade_Dashboard
{
    public partial class AnalyticsUSC : UserControl
    {

        //bar charts

        public ISeries[] CriminalCaseSeries { get; set; }
        public Axis[] CriminalXAxes { get; set; }
        public Axis[] CriminalYAxes { get; set; }

        //line charts
        public ISeries[] MonthlySeries { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        //Pie charts
        public IEnumerable<ISeries> popVSCase { get; set; }
        public IEnumerable<ISeries> genderRatioChart { get; set; }

        public IEnumerable<ISeries> caseStatChart { get; set; }
        public AnalyticsUSC()
        {
            InitializeComponent();


            //pie charts
            popVSCase = new ISeries[]
 {
    new PieSeries<double>
    {
        Values = new double[] { 20 },
        Name = "Offenders",
        Fill = new SolidColorPaint(new SKColor(0x33, 0x8b, 0x19)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    },
    new PieSeries<double>
    {
        Values = new double[] { 60 },
        Name = "Population",
        Fill = new SolidColorPaint(new SKColor(0x2b, 0x4a, 0x40)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    }
 };

            genderRatioChart = new ISeries[]
            {
    new PieSeries<double>
    {
        Values = new double[] { 20 },
        Name = "Male",
        Fill = new SolidColorPaint(new SKColor(0x33, 0x8b, 0x19)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    },
    new PieSeries<double>
    {
        Values = new double[] { 60 },
        Name = "Female",
        Fill = new SolidColorPaint(new SKColor(0x2b, 0x4a, 0x40)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    }
            };

            caseStatChart = new ISeries[]
            {
    new PieSeries<double>
    {
        Values = new double[] { 250 },
        Name = "Pending",
        Fill = new SolidColorPaint(new SKColor(0x33, 0x8b, 0x19)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    },
    new PieSeries<double>
    {
        Values = new double[] { 400 },
        Name = "Active",
        Fill = new SolidColorPaint(new SKColor(0x2b, 0x4a, 0x40)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    },
    new PieSeries<double>
    {
        Values = new double[] { 150 },
        Name = "Graduated",
        Fill = new SolidColorPaint(new SKColor(0xd8, 0xff, 0xcd)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    },
    new PieSeries<double>
    {
        Values = new double[] { 200 },
        Name = "Closed",
        Fill = new SolidColorPaint(new SKColor(0x10, 0xb9, 0x7c)),
        MaxRadialColumnWidth = 60,
        DataLabelsPosition = PolarLabelsPosition.Outer,
        DataLabelsSize = 15,
        DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
        ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}"
    }
            };


            //line chart

            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var values = new double[] { 2000, 500, 4000, 2500, 1500, 4500, 3000, 3500, 5000, 2000, 1500, 3000 };

            MonthlySeries = new ISeries[]
            {
            new LineSeries<double>
            {
                Values = values,
                Fill = new SolidColorPaint(new SKColor(0xF4, 0xFF, 0xF0)), // #f4fff0
                Stroke = new SolidColorPaint(new SKColor(0x33, 0x8B, 0x19), 2), // #338b19 with thickness 2
                GeometrySize = 10,
                GeometryStroke = new SolidColorPaint(SKColors.Black),
                DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                DataLabelsSize = 16,
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top
            }
            };

            XAxes = new Axis[]
            {
            new Axis
            {
                Labels = months,
                LabelsRotation = 15,
                Name = "Month"
            }
            };

            YAxes = new Axis[]
            {
            new Axis
            {
                Name = "Count",
                MinLimit = 0,
                MaxLimit = 5000,
                UnitWidth = 500,
                SeparatorsPaint = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 1 },
                Labeler = value => $"{value:n0}"
            }
            };

            OnPropertyChanged(nameof(MonthlySeries));
            OnPropertyChanged(nameof(XAxes));
            OnPropertyChanged(nameof(YAxes));


            //bar charts

            var criminalCases = new[]
    {
        "Bouncing Check", "Illegal Cockfighting", "Illegal Sale", "Jueteng", "Homicide",
        "Hacking", "Slight Physical Injuries", "Illegal Possession", "Murder", "Psychological Abuse",
        "Shoplifting", "Carnapping", "Swindling", "Armed Robbery", "Phishing",
        "Snatching", "Serious Physical Injuries", "Identity Theft", "Physical Abuse", "Online Libel",
        "Pickpocketing", "Economic Sabotage", "Holdup", "Hazing", "Online Scam", "Credit Card Fraud"
    };

            var counts = new double[]
            {
        45, 32, 38, 40, 25, 22, 48, 39, 18, 36,
        51, 41, 60, 37, 29, 33, 43, 30, 35, 31,
        42, 27, 28, 21, 39, 30
            };

            CriminalCaseSeries = new ISeries[]
            {
        new ColumnSeries<double>
        {
            Values = counts,
            Fill = new SolidColorPaint(new SKColor(0x2b, 0x4a, 0x40)), // #2b4a40
            Stroke = new SolidColorPaint(new SKColor(0x2b, 0x4a, 0x40), 1),
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 10,
            DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
            DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue}"
        }
            };

            CriminalXAxes = new Axis[]
            {
        new Axis
        {
            Labels = criminalCases,
            LabelsRotation = 90,
            Name = "Criminal Cases",
            NamePaint = new SolidColorPaint(SKColors.Black),
            LabelsPaint = new SolidColorPaint(SKColors.Black),
            SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
            TextSize = 20,
             ForceStepToMin = true, // Force showing all labels
            MinStep = 1
        }
            };

            CriminalYAxes = new Axis[]
            {
        new Axis
        {
            Name = "Count",
            NamePaint = new SolidColorPaint(SKColors.Black),
            LabelsPaint = new SolidColorPaint(SKColors.Black),
            MinLimit = 0,
            MaxLimit = 100,
            UnitWidth = 10,
            SeparatorsPaint = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 1 },
            Labeler = value => $"{value:n0}"
        }
            };

            OnPropertyChanged(nameof(CriminalCaseSeries));
            OnPropertyChanged(nameof(CriminalXAxes));
            OnPropertyChanged(nameof(CriminalYAxes));


            DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all tags
            Tab1Btn.Tag = null;
            Tab2Btn.Tag = null;
            Tab3Btn.Tag = null;

            // Collapse all tab contents
            Tab1Content.Visibility = Visibility.Collapsed;
            Tab2Content.Visibility = Visibility.Collapsed;
            Tab3Content.Visibility = Visibility.Collapsed;

            // Show selected tab and set active tag
            if (sender == Tab1Btn)
            {
                Tab1Content.Visibility = Visibility.Visible;
                Tab1Btn.Tag = "Active";
            }
            else if (sender == Tab2Btn)
            {
                Tab2Content.Visibility = Visibility.Visible;
                Tab2Btn.Tag = "Active";
            }
            else if (sender == Tab3Btn)
            {
                Tab3Content.Visibility = Visibility.Visible;
                Tab3Btn.Tag = "Active";
            }
        }

      
    }
}
